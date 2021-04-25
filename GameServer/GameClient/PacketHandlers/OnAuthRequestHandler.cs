using System;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameClient.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthRequest)]
    public class OnAuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_OnAuthRequest)packet;
            Console.WriteLine($"[{nameof(Net_OnAuthRequest)}] received!");
        }
    }
}
