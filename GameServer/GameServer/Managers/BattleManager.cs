using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Models.Battle;
using NetworkingShared.Packets.Battle;

namespace GameServer.Managers
{
    public class BattleManager
    {
        private static BattleManager _instance;

        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }

                return _instance;
            }
        }

        private List<Battle> _activeBattles;

        public void Initialize()
        {
            _activeBattles = new List<Battle>();
            // TODO: GenerateDummyBattle();
        }

        public void RegisterBattle(Battle battle)
        {
            this._activeBattles.Add(battle);
        }

        public Battle GetBattleById(Guid battleId)
        {
            return this._activeBattles.FirstOrDefault(x => x.Id == battleId);
        }

        public List<Battle> GetBattles()
        {
            return _activeBattles;
        }

        public void EndBattle(Battle battle, int winnerId)
        {
            // TODO: Send Net_EndBattleEvent to both players.
            //_activeBattles.Remove(battle);
        }

        public void UnRegisterBattle(Battle battle)
        {
            _activeBattles.Remove(battle);
        }

        public Guid? GetBattleIdByUserId(int userId)
        {
            var battle = _activeBattles.FirstOrDefault(x => x.Armies.Any(y => y.UserId == userId));
            if (battle == null)
            {
                return null;
            }

            return battle.Id;
        }

        public void NullifyUnitPoints(int gameId, int heroId, int unitId, bool isDefend)
        {
            var unit = GameManager.Instance.GetUnit(gameId, unitId);

            if (unit == null)
            {
                Console.WriteLine("[ERR] Cannot find unit with Id " + unitId);
                return;
            }

            unit.MovementPoints = 0;
            unit.ActionPoints = 0;

            if (isDefend)
            {
                // todo: temporary increase armor for 1 turn.
            }
        }

        public void SwitchTurn(Battle battle)
        {
            var nextArmy = battle.SwitchTurn();

            //battle.LastTurnStartTime = Time.time;
            battle.LastTurnStartTime = DateTime.UtcNow; // TODO: Not tested

            var logMessage = $"[Switching turns] Current player: {nextArmy.Name}, " +
                $"ArmyId: {battle.CurrentArmy.Id}, UnitId: {battle.CurrentUnit.Id}";

            Console.WriteLine(logMessage);
            battle.Log.Add(logMessage);

            this.SendSwitchTurnEvent(battle);
        }

        private void SendSwitchTurnEvent(Battle battle)
        {
            Net_SwitchTurnEvent msg = new Net_SwitchTurnEvent();
            msg.BattleId = battle.Id;
            msg.CurrentUnitId = battle.CurrentUnit.Id;
            msg.CurrentArmyId = battle.CurrentArmy.Id;

            foreach (var army in battle.Armies)
            {
                if (!army.IsNPC && army.Avatar != null && !army.Avatar.IsDisconnected)
                {
                    NetworkServer.Instance.Send(army.User.Connection.ConnectionId, msg);
                }
            }
        }
    }
}
