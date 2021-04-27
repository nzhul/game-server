using System;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.Battle;

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
