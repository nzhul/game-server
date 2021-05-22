using System.Linq;
using GameServer.Models;
using NetworkingShared;
using NetworkShared.Models;

namespace GameServer.PacketHandlers
{
    public class MovementHandlerBase
    {
        protected void NotifyClientsInGame(int gameId, INetPacket rmsg)
        {
            var interestedClients = NetworkServer.Instance.Connections.Where(c => c.Value.GameId == gameId);

            foreach (var client in interestedClients)
            {
                NetworkServer.Instance.Send(client.Key, rmsg);
            }
        }

        protected void UpdateCache(Army army, Coord destination, int regionId)
        {
            if (army != null)
            {
                army.X = destination.X;
                army.Y = destination.Y;
                army.GameId = regionId;
            }
        }
    }
}
