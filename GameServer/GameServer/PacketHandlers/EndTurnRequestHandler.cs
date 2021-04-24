using System;
using GameServer.Shared;
using GameServer.Shared.Packets.Battle;

namespace GameServer.PacketHandlers
{
    public class EndTurnRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_EndTurnRequest)packet;
            Console.WriteLine($"[{nameof(Net_EndTurnRequest)}] received!");
        }
    }
}
