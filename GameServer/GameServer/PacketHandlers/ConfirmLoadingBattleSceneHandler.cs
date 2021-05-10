using GameServer.Managers;
using GameServer.Models.Battle;
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
            Net_ConfirmLoadingBattleSceneRequest msg = (Net_ConfirmLoadingBattleSceneRequest)packet;

            var battle = BattleManager.Instance.GetBattleById(msg.BattleId);

            if (battle != null && battle.AttackerArmyId == msg.ArmyId)
            {
                battle.AttackerReady = true;
            }

            if (battle != null && battle.DefenderArmyId == msg.ArmyId)
            {
                battle.DefenderReady = true;
            }

            if (battle.AttackerReady && battle.DefenderReady)
            {
                battle.State = BattleState.Fight;
            }
        }
    }
}