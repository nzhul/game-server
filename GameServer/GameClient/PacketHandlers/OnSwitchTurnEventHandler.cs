using System;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.Battle;

namespace GameClient.PacketHandlers
{
    [HandlerRegister(PacketType.OnSwitchTurn)]
    public class OnSwitchTurnEventHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_SwitchTurnEvent)packet;
            Console.WriteLine($"[{nameof(Net_SwitchTurnEvent)}] received!");
        }
    }
}
