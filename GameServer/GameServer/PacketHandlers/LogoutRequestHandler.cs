using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Shared;
using GameServer.Shared.Attributes;
using GameServer.Shared.Packets.Users;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.LogoutRequest)]
    public class LogoutRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (Net_LogoutRequest)packet;
            Console.WriteLine($"[{nameof(Net_LogoutRequest)}] received!");

            if (!Server.Instance.Connections.TryGetValue(connectionId, out ServerConnection connection))
            {
                Console.WriteLine($"[WARN] Cannot find connection with id `{connectionId}`. Stop processing {nameof(Net_LogoutRequest)} message!");
                return;
            };

            Server.Instance.DisconnectUser(connection.Peer);
            Console.WriteLine($"{connection.Username} logged out from the server!");

            // TODO: Extract into FireAndForget() utility method.
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
