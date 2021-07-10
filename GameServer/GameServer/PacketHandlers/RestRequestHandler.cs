using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.RestRequest)]
    public class RestRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var connection = NetworkServer.Instance.Connections[connectionId];
            connection.User.Avatar.IsResting = true;
            GameManager.Instance.DoNewDayRestCheck(connection.UserId);
        }
    }
}
