using System;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.ReconnectRequest)]
    public class ReconnectRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Net_ReconnectRequest msg = (Net_ReconnectRequest)packet;

            // TODO: User should not be able to just reconnect to any Game.
            // Server should do API call to check what is the gameId of the user.

            var connection = NetworkServer.Instance.Connections[connectionId];
            connection.GameId = msg.GameId;

            if (GameManager.Instance.GameIsRegistered(msg.GameId))
            {
                Console.WriteLine($"Game with Id {msg.GameId} is already registered!");
                return;
            }

            var game = RequestManagerHttp.GameService.GetGame(msg.GameId);
            GameManager.Instance.RegisterGame(game);
            Console.WriteLine($"Game with Id {game.Id} loaded!");
        }
    }
}
