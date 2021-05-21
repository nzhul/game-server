using Assets.Scripts.Network.NetworkShared.Packets.World.ClientServer;
using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.LeaveGameRequest)]
    public class LeaveGameRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_LeaveGameRequest)packet;

            // TODO: Extract getting of the connection and logging into base Handle method and use try/catch.
            var connection = NetworkServer.Instance.Connections[connectionId];

            GameManager.Instance.LeaveGame(connection);
        }
    }
}
