using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Network.Services;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Managers;
using GameServer.Scheduling;
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
            Console.WriteLine("1: [SwitchTurnEvent]");
            Console.WriteLine("2: [OnAuthRequest]");
            Console.WriteLine("S: [Active connections]");
            Console.WriteLine("----");

            HandlerRegistry.Initialize((int count) => { Console.WriteLine($"{count} handlers registered!"); });
            PacketRegistry.Initialize((int count) => { Console.WriteLine($"{count} packets registered!"); });
            RequestManagerHttp.Instance.Initialize();
            RequestManagerTcp.Instance.Initialize();
            GameManager.Instance.Initialize();
            GameplayConfigurationManager.Instance.Initialize();
            Scheduler.Instance.Initialize();
            NetworkServer.Instance.Start();


            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    HandleConsoleCommand(key.Key);
                }

                NetworkServer.Instance.PollEvents();
                Scheduler.Instance.Tick();
                Thread.Sleep(15);
            }
        }

        private static void HandleConsoleCommand(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    SendSwitchTurnEvent();
                    break;
                case ConsoleKey.D2:
                    SendOnAuthRequest();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine($"Active connections: {NetworkServer.Instance.ConnectionsCount}");
                    break;
            }
        }

        private static void SendOnAuthRequest()
        {
            var randomPeer = NetworkServer.Instance.Connections.FirstOrDefault();
            NetworkServer.Instance.Send(randomPeer.Value.Peer,
                new Net_OnAuthRequest
                {
                    ConnectionId = 66,
                    ErrorMessage = "no error",
                    Success = 1
                }, DeliveryMethod.ReliableOrdered);
        }

        private static void SendSwitchTurnEvent()
        {
            var randomPeer = NetworkServer.Instance.Connections.FirstOrDefault();
            NetworkServer.Instance.Send(randomPeer.Value.Peer,
                new Net_SwitchTurnEvent
                {
                    BattleId = Guid.NewGuid(),
                    CurrentUnitId = 66,
                    Turn = Turn.Attacker
                },
                DeliveryMethod.ReliableOrdered);
        }
    }
}
