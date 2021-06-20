using System.Linq;
using GameServer.Managers;
using GameServer.NetworkShared.Models;
using GameServer.NetworkShared.Packets.World.ServerClient;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;
using Newtonsoft.Json;

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
            }

            connection.User.Avatar.IsDisconnected = false;

            var game = GameManager.Instance.GetGameByConnectionId(connectionId);

            Net_OnStartBattle rmsg = new Net_OnStartBattle
            {
                BattleId = battle.Id,
                CurrentArmyId = battle.CurrentArmy.Id,
                CurrentUnitId = battle.CurrentUnit.Id,
                Armies = battle.Armies.Select((x, i) => new ArmyParams(x.Id, i)).ToArray(),
                GameId = game.Id,
                GameString = JsonConvert.SerializeObject(game, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            };

            NetworkServer.Instance.Send(connectionId, rmsg);
        }
    }
}
