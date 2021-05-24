using System;
using GameServer.Managers;
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
            var rmsg = new Net_OnStartGame()
            {
                GameId = game.Id,
                GameString = JsonConvert.SerializeObject(game)
            };

            NetworkServer.Instance.Send(connectionId, rmsg);

            //var game = RequestManagerHttp.GameService.GetGame(msg.GameId);
            //GameManager.Instance.RegisterGame(game);
            //Console.WriteLine($"Game with Id {game.Id} loaded!");
        }
    }
}
