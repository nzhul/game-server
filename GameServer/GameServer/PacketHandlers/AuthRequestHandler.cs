using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public static event Action<ServerConnection> OnAuth;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_AuthRequest)packet;
            Console.WriteLine($"[{nameof(Net_AuthRequest)}] received!");

            Net_OnAuthRequest rmsg = new Net_OnAuthRequest();

            if (msg.IsValid())
            {
                rmsg.Success = 1;
                rmsg.ConnectionId = connectionId;

                if (!Server.Instance.Connections.TryGetValue(connectionId, out ServerConnection connection))
                {
                    Console.WriteLine($"[WARN] Cannot find connection with id `{connectionId}`. Stop processing!");
                    return;
                };

                // Update connection entity with user data.
                connection.UserId = msg.UserId;
                connection.Token = msg.Token;
                connection.Username = msg.Username;
                connection.MMR = msg.MMR;
                connection.GameId = msg.GameId;
                connection.BattleId = msg.BattleId;

                // TODO: Extract into FireAndForget() utility method.
                Task.Run(() =>
                {
                    try
                    {
                        RequestManagerHttp.UsersService.SetOnline(msg.UserId, connectionId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting user online. UserId: {connection.UserId}. Ex: {ex}");
                    }
                });

                Console.WriteLine($"{msg.Username} logged into the server!");
                OnAuth?.Invoke(connection);
            }
            else
            {
                rmsg.Success = 0;
                rmsg.ErrorMessage = "Invalid connection request!";
            }

            // NetworkServer.Instance.SendClient(recievingHostId, connectionId, rmsg);
            Server.Instance.Send(connectionId, rmsg);
        }
    }
}
