using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using GameServer.NetworkShared.Packets.World.ServerClient;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.ReconnectBattleRequest)]
    class ReconnectBattleRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Net_ReconnectBattleRequest msg = (Net_ReconnectBattleRequest)packet;

            // TODO: User should not be able to just reconnect to any Game.
            // Server should do API call to check what is the gameId of the user.

            var connection = NetworkServer.Instance.Connections[connectionId];
            connection.GameId = msg.GameId;

            var battle = BattleManager.Instance.GetBattleById(msg.BattleId);

            if (battle == null)
            {
                NetworkServer.Instance.Send(connectionId, new Net_OnReconnectBattleFail());

                // TODO: Extract into FireAndForget() utility method.
                Task.Run(() =>
                {
                    try
                    {
                        RequestManagerHttp.BattleService.UnRegisterBattle(connection.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting user offline. UserId: {connection.UserId}. Ex: {ex}");
                    }
                });
                return;
            }

            var isAttacker = battle.AttackerArmy.UserId == connection.UserId;
            if (isAttacker)
            {
                battle.AttackerDisconnected = false;
                battle.AttackerConnectionId = connectionId;
            }
            else
            {
                battle.DefenderDisconnected = false;
                battle.DefenderConnectionId = connectionId;
            }


            Net_OnStartBattle rmsg = new Net_OnStartBattle
            {
                BattleId = battle.Id,
                AttackerArmyId = battle.AttackerArmyId,
                DefenderArmyId = battle.DefenderArmyId,
                BattleScenario = battle.BattleScenario,
                SelectedUnitId = battle.SelectedUnit.Id,
                AttackerType = battle.AttackerType,
                DefenderType = battle.DefenderType,
                Turn = battle.Turn
            };

            NetworkServer.Instance.Send(connectionId, rmsg);
        }
    }
}
