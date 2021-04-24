using System;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_AuthRequest)packet;
            Console.WriteLine($"[{nameof(Net_AuthRequest)}] received!");
        }
    }
}
