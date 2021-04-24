using System;
using GameServer.Shared;
using GameServer.Shared.Packets.Battle;

namespace GameServer.PacketHandlers
{
    public class ConfirmLoadingBattleSceneHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (Net_ConfirmLoadingBattleSceneRequest)packet;
            Console.WriteLine($"[{nameof(Net_ConfirmLoadingBattleSceneRequest)}] received!");
        }
    }
}
