using GameServer.Models.Users;

namespace Assets.Scripts.Network.Services.HTTP.Interfaces
{
    public interface IUsersService
    {
        User GetUser(int id);

        void ClearBattle(int userId);

        void ClearAllBattles();

        void SetOnline(int userId, int connectionId);

        void SetOffline(int userId);

        LoginResponse LoginAdmin();
    }
}
