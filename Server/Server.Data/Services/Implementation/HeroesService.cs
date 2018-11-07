using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models.Heroes;

namespace Server.Data.Services.Implementation
{
    public class HeroesService : BaseService, IHeroesService
    {
        public HeroesService(DataContext context)
            : base(context)
        {
        }

        public async Task<bool> DeleteHero(int heroId)
        {
            Hero dbHero = await _context.Heroes.FirstOrDefaultAsync(h => h.Id == heroId);

            if (dbHero != null)
            {
                // TODO -> delete items and other hero stuff here before deleting the hero
                // Update Avatar if needed

                base.Delete<Hero>(dbHero);
                return await base.SaveAll();
            }

            return false;
        }

        public async Task<Hero> GetHeroWithRegion(int heroId)
        {
            return await _context.Heroes.Include(h => h.Region).FirstOrDefaultAsync(h => h.Id == heroId);
        }

        public async Task<Hero> UpdateHeroPosition(Hero hero, int x, int y)
        {
            hero.X = x;
            hero.Y = y;

            await base.SaveAll();

            return hero;
        }
    }
}
