using System;
using System.Threading;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Shared;
using GameServer.Shared.Packets.Battle;
using GameServer.Shared.Packets.Users;
using LiteNetLib;

namespace GameClient
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            Console.WriteLine("--Available commands --");
            Console.WriteLine("1: [StartBattleRequest]");
            Console.WriteLine("2: [ConfirmLoadingBattleSceneRequest]");
            Console.WriteLine("3: [EndTurnRequest]");
            Console.WriteLine("4: [AuthRequest]");
            Console.WriteLine("5: [LogoutRequest]");
            Console.WriteLine("----");
            HandlerRegistry.Initialize();
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
                case ConsoleKey.D4:
                    SendAuthRequest();
                    break;
                case ConsoleKey.D5:
                    SendLogoutRequest();
                    break;
            }
        }

        private static void SendLogoutRequest()
        {
            var p = new Net_LogoutRequest();
            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
        }

        private static void SendAuthRequest()
        {
            // TODO: Test with null battle id
            var p = new Net_AuthRequest
            {
                UserId = 3,
                Username = "nzhul",
                MMR = 0,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
                GameId = 1
            };

            client.SendPacketSerializable(p, DeliveryMethod.ReliableOrdered);
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
