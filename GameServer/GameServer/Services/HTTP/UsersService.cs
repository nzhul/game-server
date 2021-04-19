using System;
using Assets.Scripts.Network.Services.HTTP.Interfaces;
using GameServer.Shared.Users;

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
    }
}
