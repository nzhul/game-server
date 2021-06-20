using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Managers;
using GameServer.Models.Users;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public static event Action<ServerConnection> OnAuth;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_AuthRequest)packet;
            Net_OnAuthRequest rmsg = new Net_OnAuthRequest();

            if (msg.IsValid())
            {
                rmsg.Success = 1;
                rmsg.ConnectionId = connectionId;

                if (!NetworkServer.Instance.Connections.TryGetValue(connectionId, out ServerConnection connection))
                {
                    Console.WriteLine($"[WARN] Cannot find connection with id `{connectionId}`. Stop processing!");
                    return;
                };

                // Update connection entity with user data.
                connection.UserId = msg.UserId;

                // if game exist with this user, get the user object from there instead.
                var game = GameManager.Instance.GetGameByUserId(connection.UserId);
                if (game != null)
                {
                    var existingUser = game.Avatars.First(x => x.UserId == connection.UserId).User;
                    connection.User = existingUser;
                    existingUser.Connection = connection;
                    existingUser.Avatar.IsDisconnected = false;
                }
                else
                {
                    connection.User = new User
                    {
                        Id = msg.UserId,
                        Username = msg.Username,
                        Mmr = msg.MMR,
                        Token = msg.Token,
                        Connection = connection,
                    };
                }

                // TODO: Delete this temporary method.
                //GameManager.Instance.RelinkGameUserAndAvatarTMP(connection.User);

                rmsg.GameId = GameManager.Instance.GetGameIdByUserId(connection.UserId);
                rmsg.BattleId = BattleManager.Instance.GetBattleIdByUserId(connection.UserId);

                connection.GameId = rmsg.GameId;
                connection.BattleId = rmsg.BattleId;

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
            NetworkServer.Instance.Send(connectionId, rmsg);
        }
    }
}
