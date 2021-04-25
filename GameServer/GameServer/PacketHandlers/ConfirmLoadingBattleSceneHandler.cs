using System;
using GameServer.Shared;
using GameServer.Shared.Attributes;
using GameServer.Shared.Packets.Battle;

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
