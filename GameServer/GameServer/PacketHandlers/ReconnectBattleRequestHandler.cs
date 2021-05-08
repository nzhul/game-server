using GameServer.Managers;
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

            //var battle = NetworkServer.Instance.ActiveBattles.FirstOrDefault(x => x.Id == msg.BattleId);
            var battle = BattleManager.Instance.GetBattleById(msg.BattleId);

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
