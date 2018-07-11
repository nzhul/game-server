using System.Threading.Tasks;

namespace Server.Data.Services.Abstraction
{
    public interface IHeroesService : IService
    {
        Task<bool> DeleteHero(int heroId);
    }
}
