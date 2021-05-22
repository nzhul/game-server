using System;
using System.Threading.Tasks;

namespace Server.Data.Services.Abstraction
{
    public interface IBattleService
    {
        Task<bool> RegisterBattle(Guid battleId, int userId);

        Task<bool> UnRegisterBattle(int userId);
    }
}
