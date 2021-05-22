using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Models.Battle;
using NetworkingShared.Packets.Battle;
using NetworkShared.Enums;

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

        private List<Battle> _activeBattles = new List<Battle>();

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

        public void DisconnectFromBattle(int connectionId)
        {
            var attackerBattle = _activeBattles.FirstOrDefault(x => x.AttackerConnectionId == connectionId);

            if (attackerBattle != null)
            {
                attackerBattle.AttackerDisconnected = true;
                attackerBattle.AttackerConnectionId = -1;
            }

            var defenderBattle = _activeBattles.FirstOrDefault(x => x.AttackerConnectionId == connectionId);

            if (defenderBattle != null)
            {
                defenderBattle.AttackerDisconnected = true;
                defenderBattle.AttackerConnectionId = -1;
            }
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
            if (battle.Turn == Turn.Attacker)
            {
                battle.Turn = Turn.Defender;
                battle.SelectedUnit = GameManager.Instance.GetRandomAvailibleUnit(battle.DefenderArmy);
            }
            else
            {
                battle.Turn = Turn.Attacker;
                battle.SelectedUnit = battle.SelectedUnit = GameManager.Instance.GetRandomAvailibleUnit(battle.AttackerArmy);
            }

            //battle.LastTurnStartTime = Time.time;
            battle.LastTurnStartTime = DateTime.UtcNow; // TODO: Not tested
            Console.WriteLine("Switching turns! New Player is: " + battle.Turn);
            battle.Log.Add("Switching turns! New Player is: " + battle.Turn);
            this.SendSwitchTurnEvent(battle);
        }

        private void SendSwitchTurnEvent(Battle battle)
        {
            // TODO: Maybe move the sending in the TCP BattleService or completely delete the TCP Service ?
            Net_SwitchTurnEvent msg = new Net_SwitchTurnEvent();
            msg.BattleId = battle.Id;
            msg.CurrentUnitId = battle.SelectedUnit.Id;
            msg.Turn = battle.Turn;

            bool shouldNotifyAttacker = !battle.AttackerDisconnected && battle.AttackerType == PlayerType.Human;
            bool shouldNotifyDefender = !battle.DefenderDisconnected && battle.DefenderType == PlayerType.Human;

            if (shouldNotifyAttacker)
            {
                NetworkServer.Instance.Send(battle.AttackerConnectionId, msg);
            }

            if (shouldNotifyDefender)
            {
                NetworkServer.Instance.Send(battle.DefenderConnectionId, msg);
            }
        }
    }
}
