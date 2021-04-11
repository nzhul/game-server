using System;
using System.Threading;
using GameServer.Shared;
using LiteNetLib;

namespace GameClient
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            client = new Client();
            client.Connect();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    HandleConsoleCommand(key.Key);
                }

                client.PollEvents();
                Thread.Sleep(15);
            }
        }

        private static void HandleConsoleCommand(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    SendStartBattleRequest();
                    break;
            }
        }

        private static void SendStartBattleRequest()
        {
            var p = new StartBattleRequest { AttackerArmyId = 22, DefenderArmyId = 33 };
            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
        }
    }
}
