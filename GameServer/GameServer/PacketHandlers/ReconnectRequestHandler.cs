using System;
using GameServer.Managers;
using GameServer.Models.View;
using GameServer.Utilities;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;
using Newtonsoft.Json;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.ReconnectRequest)]
    public class ReconnectRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Net_ReconnectRequest msg = (Net_ReconnectRequest)packet;

            var connection = NetworkServer.Instance.Connections[connectionId];
            connection.GameId = msg.GameId;

            if (!GameManager.Instance.GameIsRegistered(msg.GameId))
            {
                Console.WriteLine($"[ERROR] Game with Id {msg.GameId} is not registered registered!");
                // TODO: Return fail event ?
                return;
            }

            // raise startGame event

            var game = GameManager.Instance.GetGameByConnectionId(connectionId);
            var simpleGame = AM.Instance.Mapper.Map<GameDetailedDto>(game);
            var rmsg = new Net_OnStartGame()
            {
                GameId = game.Id,
                // !!! TODO: I need to not serialize directly the game model
                // !!! TODO: Use simpler model instead / automapper.
                GameString = JsonConvert.SerializeObject(simpleGame, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            };

            NetworkServer.Instance.Send(connectionId, rmsg);

            //var game = RequestManagerHttp.GameService.GetGame(msg.GameId);
            //GameManager.Instance.RegisterGame(game);
            //Console.WriteLine($"Game with Id {game.Id} loaded!");
        }
    }
}
