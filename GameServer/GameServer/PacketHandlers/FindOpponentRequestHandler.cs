using Assets.Scripts.Network.Services;
using GameServer.Matchmaking;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_FindOpponentRequest)packet;

            // Get player latest mmr from API
            // register the player in NetworkServer.Instance.MatchmakingPool

            var connection = NetworkServer.Instance.Connections[connectionId];
            var userData = RequestManagerHttp.UsersService.GetUser(connection.UserId);
            connection.User.Mmr = userData.Mmr;

            Matchmaker.Instance.RegisterPlayer(connection, msg.Class);
        }
    }
}
