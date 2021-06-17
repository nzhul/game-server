using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.Users;

namespace GameServer.PacketHandlers
{
    /// <summary>
    /// When user logs out from the server we don't disconnect him from the server, we just set him as offline in the API.
    /// </summary>
    [HandlerRegister(PacketType.LogoutRequest)]
    public class LogoutRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_LogoutRequest)packet;

            // TODO: Extract getting of the connection and logging into base Handle method and use try/catch.
            if (!NetworkServer.Instance.Connections.TryGetValue(connectionId, out ServerConnection connection))
            {
                Console.WriteLine($"[WARN] Cannot find connection with id `{connectionId}`. Stop processing {nameof(Net_LogoutRequest)} message!");
                return;
            };

            Console.WriteLine($"{connection.User.Username} logged out from the server!");
            GameManager.Instance.DisconnectFromGame(connection.UserId);

            Task.Run(() =>
            {
                try
                {
                    RequestManagerHttp.UsersService.SetOffline(connection.UserId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting user offline. UserId: {connection.UserId}. Ex: {ex}");
                }
            });
        }
    }
}
