﻿using System;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameClient.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthRequest)]
    public class OnAuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_OnAuthRequest)packet;
            Console.WriteLine($"[{nameof(Net_OnAuthRequest)}] received!");
        }
    }
}
