using System;
using Assets.Scripts.Network.Services.HTTP.Interfaces;
using GameServer.Configuration;
using NetworkingShared.Users;

namespace Assets.Scripts.Network.Services.HTTP
{
    public class UsersService : BaseService, IUsersService
    {
        public User GetUser(int id)
        {
            return base.Get<User>($"users/{id}");
        }

        public void ClearBattle(int userId)
        {
            base.Put($"users/{userId}/clearbattle");
        }

        public void ClearAllBattles()
        {
            Console.WriteLine("User Battles Cleared!");
            base.Put($"users/clearallbattles");
        }

        public void SetOnline(int userId, int connectionId)
        {
            base.Put($"users/{userId}/setonline/{connectionId}");
        }

        public void SetOffline(int userId)
        {
            base.Put($"users/{userId}/setoffline");
        }

        public LoginResponse LoginAdmin()
        {
            var loginInput = new
            {
                username = ConfigProvider.Instance.ServerConfigraution.AdminName,
                password = ConfigProvider.Instance.ServerConfigraution.AdminPassword
            };

            return base.Post<LoginResponse>("auth/login", loginInput);
        }
    }
}
