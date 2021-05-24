using System;
using System.Linq;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Models.Battle;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;
using NetworkShared.Enums;

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

            if (msg.IsValid())
            {
                var scenario = this.ResolveBattleScenario(msg.AttackerType, msg.DefenderType);
                rmsg.AttackerArmyId = msg.AttackerArmyId;
                rmsg.DefenderArmyId = msg.DefenderArmyId;
                rmsg.BattleScenario = scenario;

                // 1. Add new record into NetworkServer.Instance.ActiveBattles
                // 2. Send OnStartBattle back to the client.

                Battle newBattle = new Battle()
                {
                    Id = Guid.NewGuid(),
                    GameId = game.Id,
                    AttackerArmyId = msg.AttackerArmyId,
                    DefenderArmyId = msg.DefenderArmyId,
                    AttackerArmy = game.Armies.FirstOrDefault(x => x.Id == msg.AttackerArmyId),
                    DefenderArmy = game.Armies.FirstOrDefault(x => x.Id == msg.DefenderArmyId),
                    AttackerType = msg.AttackerType,
                    DefenderType = msg.DefenderType,
                    BattleScenario = scenario,
                    LastTurnStartTime = DateTime.UtcNow
                };

                newBattle.SelectedUnit = GameManager.Instance.GetRandomAvailibleUnit(newBattle.AttackerArmy);

                this.UpdateUnitsData(newBattle.AttackerArmy);
                this.UpdateUnitsData(newBattle.DefenderArmy);

                rmsg.BattleId = newBattle.Id;
                rmsg.SelectedUnitId = newBattle.SelectedUnit.Id;
                rmsg.Turn = Turn.Attacker;

                this.ConfigurePlayerReady(newBattle, scenario);
                newBattle.AttackerConnectionId = GameManager.Instance.GetConnectionIdByArmyId(game.Id, newBattle.AttackerArmyId);
                newBattle.DefenderConnectionId = GameManager.Instance.GetConnectionIdByArmyId(game.Id, newBattle.DefenderArmyId);

                BattleManager.Instance.RegisterBattle(newBattle);
                NetworkServer.Instance.Send(connectionId, rmsg);

                // TODO: Delete this.
                //Task.Run(() =>
                //{
                //    var connection = NetworkServer.Instance.Connections[connectionId];
                //    try
                //    {
                //        RequestManagerHttp.BattleService.RegisterBattle(newBattle.Id, connection.UserId);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"Error registering battle on API. UserId: {connection.UserId}. Ex: {ex}");
                //    }
                //});

                // TODO: Register battle for other player when pvp battle.
            }
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

        private void ConfigurePlayerReady(Battle newBattle, BattleScenario scenario)
        {
            switch (scenario)
            {
                case BattleScenario.HUvsAI:
                    newBattle.AttackerReady = false;
                    newBattle.DefenderReady = true;
                    break;
                case BattleScenario.AIvsAI:
                    newBattle.AttackerReady = true;
                    newBattle.DefenderReady = true;
                    break;
                case BattleScenario.HUvsHU:
                    newBattle.AttackerReady = false;
                    newBattle.DefenderReady = false;
                    break;
                case BattleScenario.AIvsHU:
                    newBattle.AttackerReady = true;
                    newBattle.DefenderReady = false;
                    break;
                default:
                    break;
            }
        }

        private BattleScenario ResolveBattleScenario(PlayerType attackerType, PlayerType defenderType)
        {
            if (attackerType == PlayerType.Human && defenderType == PlayerType.AI)
            {
                return BattleScenario.HUvsAI;
            }
            else if (attackerType == PlayerType.AI && defenderType == PlayerType.AI)
            {
                return BattleScenario.AIvsAI;
            }
            else if (attackerType == PlayerType.Human && defenderType == PlayerType.Human)
            {
                return BattleScenario.HUvsHU;
            }
            else if (attackerType == PlayerType.AI && defenderType == PlayerType.Human)
            {
                return BattleScenario.AIvsHU;
            }

            return BattleScenario.Unknown;
        }
    }
}
