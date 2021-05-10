using System;
using GameServer.Managers;
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
            Net_EndTurnRequest msg = (Net_EndTurnRequest)packet;
            var game = GameManager.Instance.GetGameByConnectionId(connectionId);

            // 1. Find the battle
            var battle = BattleManager.Instance.GetBattleById(msg.BattleId);

            if (battle == null)
            {
                Console.WriteLine($"Cannot find battle with Id {msg.BattleId}");
                return;
            }

            //if (battle.CurrentUnitId != msg.RequesterUnitId)
            //{
            //    Debug.LogWarning($"End turn requester is not currently active! Hacking ?");
            //    return;
            //}

            // 2. Update last activity
            battle.UpdateLastActivity(msg.RequesterArmyId);

            // 3. Set movement and action points to zero

            if (msg.RequesterUnitId != 0)
            {
                //this.battleService.NullifyUnitPoints(game.Id, msg.RequesterArmyId, msg.RequesterUnitId, msg.IsDefend);
                BattleManager.Instance.NullifyUnitPoints(game.Id, msg.RequesterArmyId, msg.RequesterUnitId, msg.IsDefend);
            }


            // 4. Switch turns if the remaining time is more than 5 seconds
            BattleManager.Instance.SwitchTurn(battle);
        }
    }
}
