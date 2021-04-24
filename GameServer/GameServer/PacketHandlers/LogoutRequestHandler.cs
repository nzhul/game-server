using System;
using GameServer.Shared;
using GameServer.Shared.Attributes;
using GameServer.Shared.Packets.Users;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.LogoutRequest)]
    public class LogoutRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_LogoutRequest)packet;
            Console.WriteLine($"[{nameof(Net_LogoutRequest)}] received!");
        }
    }
}
