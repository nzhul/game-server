using System;
using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.CancelFindOpponentRequest)]
    public class CancelFindOpponentRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
