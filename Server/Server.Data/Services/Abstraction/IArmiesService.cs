using System.Threading.Tasks;
using Server.Models.Armies;

namespace Server.Data.Services.Abstraction
{
    public interface IArmiesService : IService
    {
        Task<Army> GetArmy(int heroId);

        Task<bool> DeleteHero(int heroId);

        Task<bool> UpdateArmyPosition(int armyId, int x, int y);
    }
}
