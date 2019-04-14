using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models.Heroes;
using System.Threading.Tasks;

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

        public async Task<Hero> GetHero(int heroId)
        {
            return await _context.Heroes.FirstOrDefaultAsync(h => h.Id == heroId);
        }

        public async Task<Hero> UpdateHeroPosition(Hero hero, int x, int y, int regionId)
        {
            hero.X = x;
            hero.Y = y;

            if (hero.RegionId != regionId)
            {
                var oldRegion = await _context.Regions.FirstOrDefaultAsync(r => r.Id == hero.RegionId);
                oldRegion.Heroes.Remove(hero);

                var newRegion = await _context.Regions.FirstOrDefaultAsync(r => r.Id == regionId);
                newRegion.Heroes.Add(hero);
            }

            await base.SaveAll();

            return hero;
        }
    }
}
