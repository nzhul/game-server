using System.Threading.Tasks;
using Server.Models.Heroes;

namespace Server.Data.Services.Abstraction
{
    public interface IHeroesService : IService
    {
        Task<Hero> GetHeroWithRegion(int heroId);

        Task<bool> DeleteHero(int heroId);

        Task<Hero> UpdateHeroPosition(Hero hero, int x, int y);
    }
}
