using System;
using System.Linq;
using System.Threading;
using GameServer.Shared;
using LiteNetLib;

namespace GameServer
{
    class Program
    {
        static Server _server;

        static void Main(string[] args)
        {
            _server = new Server();
            _server.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    HandleConsoleCommand(key.Key);
                }

                _server.PollEvents();
                Thread.Sleep(15);
            }
        }

        private static void HandleConsoleCommand(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D2:
                    SendEndBattleEvent();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine($"Active connections: {_server.ConnectionsCount}");
                    break;
            }
        }

        private static void SendEndBattleEvent()
        {
            var randomPeer = _server.Clients.FirstOrDefault();
            _server.Send(randomPeer, new EndBattleEvent { BattleId = 99 }, DeliveryMethod.ReliableOrdered);
        }
    }
}
