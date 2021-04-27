using System;
using Assets.Scripts.Network.Services.TCP.Interfaces;
using GameServer.Games;
using GameServer.Models.Battle;
using GameServer.Utilities;
using NetworkingShared.Packets.Battle;
using NetworkShared.Enums;

namespace Assets.Scripts.Network.Services.TCP
{
    public class BattleService : IBattleService
    {
        public void EndBattle(Battle battle, int winnerId)
        {
            if (winnerId <= 0)
            {
                // both players are loosers
                // send two defeat requests
            }

            if (battle.AttackerArmy.Id == winnerId)
            {
                // attacker is winner
                // defender is defeated
                // send win to attacker and defeat to defender
            }
            else if (battle.DefenderArmy.Id == winnerId)
            {
                // attacker is defeated
                // defender is winner
                // send defeat to attacker and win to defender
            }

            // DO API call to save battle outcome

            // TODO: Clear both users - BattleId
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
            battle.LastTurnStartTime = Timer.GetElapsedTime(); // TODO: Not tested
            Console.WriteLine("Switching turns! New Player is: " + battle.Turn);
            battle.Log.Add("Switching turns! New Player is: " + battle.Turn);
            this.SendSwitchTurnEvent(battle);
        }

        private void SendSwitchTurnEvent(Battle battle)
        {
            Net_SwitchTurnEvent msg = new Net_SwitchTurnEvent();
            msg.BattleId = battle.Id;
            msg.CurrentUnitId = battle.SelectedUnit.Id;
            msg.Turn = battle.Turn;

            bool shouldNotifyAttacker = !battle.AttackerDisconnected && battle.AttackerType == PlayerType.Human;
            bool shouldNotifyDefender = !battle.DefenderDisconnected && battle.DefenderType == PlayerType.Human;

            if (shouldNotifyAttacker)
            {
                //TODO: Invoke server.
                //NetworkServer.Instance.SendClient(0, battle.AttackerConnectionId, msg);
            }

            if (shouldNotifyDefender)
            {
                //TODO: Invoke server.
                //NetworkServer.Instance.SendClient(0, battle.DefenderConnectionId, msg);
            }
        }
    }
}
