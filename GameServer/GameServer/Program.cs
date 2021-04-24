using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Network.Services;
using GameServer.Games;
using GameServer.PacketHandlers;
using GameServer.Shared;
using GameServer.Shared.Models;
using GameServer.Shared.Packets.Battle;
using LiteNetLib;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--Available commands --");
            Console.WriteLine("1: [EndBattleEvent]");
            Console.WriteLine("2: [SwitchTurnEvent]");
            Console.WriteLine("S: [Active connections]");
            Console.WriteLine("----");

            HandlerRegistry.Initialize();
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
                case ConsoleKey.D2:
                    SendSwitchTurnEvent();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine($"Active connections: {Server.Instance.ConnectionsCount}");
                    break;
            }
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
