using System;
using GameServer.Shared;
using GameServer.Shared.Attributes;
using GameServer.Shared.Packets.Battle;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.EndTurnRequest)]
    public class EndTurnRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_EndTurnRequest)packet;
            Console.WriteLine($"[{nameof(Net_EndTurnRequest)}] received!");
        }
    }
}
