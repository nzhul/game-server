using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Models.Battle;
using GameServer.NetworkShared.Models;
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
            attackerArmy.Order = 0;
            defenderArmy.Order = 1;
            attackerArmy.LastActivity = DateTime.UtcNow;
            defenderArmy.LastActivity = DateTime.UtcNow;

            armies.Add(attackerArmy);
            armies.Add(defenderArmy);

            Battle newBattle = new Battle()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Armies = armies,
                LastTurnStartTime = DateTime.UtcNow
            };

            newBattle.CurrentArmy = attackerArmy;
            newBattle.CurrentUnit = newBattle.GetRandomAvailibleUnit(attackerArmy);

            this.UpdateUnitsData(attackerArmy);
            this.UpdateUnitsData(defenderArmy);

            rmsg.BattleId = newBattle.Id;
            rmsg.CurrentArmyId = newBattle.CurrentArmy.Id;
            rmsg.CurrentUnitId = newBattle.CurrentUnit.Id;
            rmsg.Armies = armies.Select(x => new ArmyParams(x.Id, x.Order)).ToArray();

            BattleManager.Instance.RegisterBattle(newBattle);
            NetworkServer.Instance.Send(connectionId, rmsg);

            if (!defenderArmy.IsNPC)
            {
                var defenderConnectionId = GameManager.Instance.GetConnectionIdByArmyId(game.Id, defenderArmy.Id);
                if (defenderConnectionId > -1)
                {
                    
                    NetworkServer.Instance.Send(defenderConnectionId, rmsg);
                }

                // if connectionId is -1 - this means that the user is disconnected and we don't need to send message.
            }

            Console.WriteLine($"Starting battle between {attackerArmy.Name} and {defenderArmy.Name}. " +
                $"Current player: {attackerArmy.Name}, ArmyId: {rmsg.CurrentArmyId}, UnitId: {rmsg.CurrentUnitId}");
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
