using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Network.Services;
using GameServer.Games;
using GameServer.Shared;
using LiteNetLib;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            PacketRegistry.Initialize();
            RequestManagerHttp.Instance.Initialize();
            GameManager.Instance.Initialize();
            // TODO: Invoke RequestManagerHttp.Instance.UpdateHeaders(headers); after admin login
            Server.Instance.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    HandleConsoleCommand(key.Key);
                }

                Server.Instance.PollEvents();
                Thread.Sleep(15);
            }
        }

        private static void HandleConsoleCommand(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    SendEndBattleEvent();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine($"Active connections: {Server.Instance.ConnectionsCount}");
                    break;
            }
        }

        private static void SendEndBattleEvent()
        {
            var randomPeer = Server.Instance.Connections.FirstOrDefault();
            Server.Instance.Send(randomPeer.Value.Peer, new EndBattleEvent { BattleId = 99 }, DeliveryMethod.ReliableOrdered);
        }
    }
}
