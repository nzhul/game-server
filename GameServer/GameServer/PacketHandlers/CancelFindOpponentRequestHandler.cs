using GameServer.Matchmaking;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.CancelFindOpponentRequest)]
    public class CancelFindOpponentRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = NetworkServer.Instance.Connections[connectionId];

            Matchmaker.Instance.UnRegisterPlayer(connection);
        }
    }
}
