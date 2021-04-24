﻿using System;
using GameServer.Shared;
using GameServer.Shared.Packets.Battle;

namespace GameClient.PacketHandlers
{
    public class OnSwitchTurnEventHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_SwitchTurnEvent)packet;
            Console.WriteLine($"[{nameof(Net_SwitchTurnEvent)}] received!");
        }
    }
}