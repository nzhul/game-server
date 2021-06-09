﻿using System;
using System.Linq;
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

            var army = battle.Armies.FirstOrDefault(x => x.Id == msg.ArmyId);

            if (army == null)
            {
                throw new ArgumentException($"Army with Id {msg.ArmyId} cannot be found");
            }

            army.ReadyForBattle = msg.IsReady;
            if (battle.Armies.All(x => x.ReadyForBattle))
            {
                battle.State = BattleState.Fight;
            }
        }
    }
}