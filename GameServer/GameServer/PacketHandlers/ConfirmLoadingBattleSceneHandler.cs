using System;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.Battle;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.ConfirmLoadingBattleScene)]
    public class ConfirmLoadingBattleSceneHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_ConfirmLoadingBattleSceneRequest)packet;
            Console.WriteLine($"[{nameof(Net_ConfirmLoadingBattleSceneRequest)}] received!");
        }
    }
}
