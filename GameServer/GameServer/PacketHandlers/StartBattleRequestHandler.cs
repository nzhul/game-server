using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Models.Battle;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.StartBattleRequest)]
    public class StartBattleRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_StartBattleRequest)packet;

            Net_OnStartBattle rmsg = new Net_OnStartBattle();
            var game = GameManager.Instance.GetGameByConnectionId(connectionId);

            var armies = new List<Army>();
            var attackerArmy = game.Armies.FirstOrDefault(x => x.Id == msg.AttackerArmyId);
            var defenderArmy = game.Armies.FirstOrDefault(x => x.Id == msg.DefenderArmyId);
            attackerArmy.TurnOrder = 1;
            defenderArmy.TurnOrder = 2;

            armies.Add(attackerArmy);
            armies.Add(defenderArmy);

            Battle newBattle = new Battle()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Armies = armies,
                LastTurnStartTime = DateTime.UtcNow
            };

            newBattle.CurrentArmyId = attackerArmy.Id;
            newBattle.CurrentUnit = newBattle.GetRandomAvailibleUnit(attackerArmy);

            this.UpdateUnitsData(attackerArmy);
            this.UpdateUnitsData(defenderArmy);

            rmsg.BattleId = newBattle.Id;
            rmsg.CurrentArmyId = newBattle.CurrentArmyId;
            rmsg.CurrentUnitId = newBattle.CurrentUnit.Id;

            BattleManager.Instance.RegisterBattle(newBattle);
            NetworkServer.Instance.Send(connectionId, rmsg);
        }

        private void UpdateUnitsData(Army Army)
        {
            //TODO apply upgrades before the battle!
            foreach (var unit in Army.Units)
            {
                var config = GameplayConfigurationManager.Instance.UnitConfigurations[unit.Type];
                unit.MovementPoints = config.MovementPoints;
                unit.MaxMovementPoints = unit.MovementPoints;
                unit.ActionPoints = config.ActionPoints;
                unit.MaxMovementPoints = unit.ActionPoints;
                unit.MinDamage = config.MinDamage;
                unit.MaxDamage = config.MaxDamage;
                unit.Hitpoints = config.Hitpoints;
                unit.BaseHitpoints = unit.Hitpoints;
                unit.MaxHitpoints = unit.Hitpoints;
                unit.Mana = config.Mana;
                unit.Armor = config.Armor;
                unit.AttackType = config.AttackType;
                unit.ArmorType = config.ArmorType;
                unit.Level = config.CreatureLevel;
            }
        }
    }
}
