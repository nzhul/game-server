using System;
using System.Threading;
using GameServer.Shared;
using GameServer.Shared.Packets.Battle;
using LiteNetLib;

namespace GameClient
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            PacketRegistry.Initialize();
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
                case ConsoleKey.D2:
                    SendConfirmLoadingBattleSceneRequest();
                    break;
                case ConsoleKey.D3:
                    SendEndTurnRequest();
                    break;
            }
        }

        private static void SendEndTurnRequest()
        {
            var p = new Net_EndTurnRequest
            {
                BattleId = Guid.NewGuid(),
                IsDefend = false,
                RequesterArmyId = 55,
                RequesterUnitId = 66,
            };

            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
        }

        private static void SendConfirmLoadingBattleSceneRequest()
        {
            var p = new Net_ConfirmLoadingBattleSceneRequest
            {
                ArmyId = 1,
                IsReady = true,
                BattleId = Guid.NewGuid(),
            };

            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
        }

        private static void SendStartBattleRequest()
        {
            var p = new StartBattleRequest { AttackerArmyId = 22, DefenderArmyId = 33 };
            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
        }
    }
}
