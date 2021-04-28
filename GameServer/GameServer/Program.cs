using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Network.Services;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Games;
using LiteNetLib;
using NetworkingShared;
using NetworkingShared.Packets.Battle;
using NetworkShared.Enums;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--Available commands --");
            Console.WriteLine("1: [EndBattleEvent]");
            Console.WriteLine("2: [SwitchTurnEvent]");
            Console.WriteLine("3: [OnAuthRequest]");
            Console.WriteLine("S: [Active connections]");
            Console.WriteLine("----");

            HandlerRegistry.Initialize((int count) => { Console.WriteLine($"{count} handlers registered!"); });
            PacketRegistry.Initialize((int count) => { Console.WriteLine($"{count} packets registered!"); });
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
                case ConsoleKey.D2:
                    SendSwitchTurnEvent();
                    break;
                case ConsoleKey.D3:
                    SendOnAuthRequest();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine($"Active connections: {Server.Instance.ConnectionsCount}");
                    break;
            }
        }

        private static void SendOnAuthRequest()
        {
            var randomPeer = Server.Instance.Connections.FirstOrDefault();
            Server.Instance.Send(randomPeer.Value.Peer,
                new Net_OnAuthRequest
                {
                    ConnectionId = 66,
                    ErrorMessage = "no error",
                    Success = 1
                }, DeliveryMethod.ReliableOrdered);
        }

        private static void SendSwitchTurnEvent()
        {
            var randomPeer = Server.Instance.Connections.FirstOrDefault();
            Server.Instance.Send(randomPeer.Value.Peer,
                new Net_SwitchTurnEvent
                {
                    BattleId = Guid.NewGuid(),
                    CurrentUnitId = 66,
                    Turn = Turn.Attacker
                },
                DeliveryMethod.ReliableOrdered);
        }

        private static void SendEndBattleEvent()
        {
            var randomPeer = Server.Instance.Connections.FirstOrDefault();
            Server.Instance.Send(randomPeer.Value.Peer, new EndBattleEvent { BattleId = 99 }, DeliveryMethod.ReliableOrdered);
        }
    }
}
