using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models.Armies;
using Server.Models.Heroes;

namespace Server.Data.Services.Implementation
{
    public class ArmiesService : BaseService, IArmiesService
    {
        public ArmiesService(DataContext context)
            : base(context)
        {
        }

        public async Task<bool> DeleteHero(int heroId)
        {
            Unit dbHero = await _context.Units.FirstOrDefaultAsync(h => h.Id == heroId);

            if (dbHero != null)
            {
                // TODO -> delete items and other hero stuff here before deleting the hero
                // Update Avatar if needed

                base.Delete<Unit>(dbHero);
                return await base.SaveAll();
            }

            return false;
        }

        public async Task<Army> GetArmy(int heroId)
        {
            return await _context.Armies.FirstOrDefaultAsync(h => h.Id == heroId);
        }

        public async Task<bool> UpdateArmyPosition(int armyId, int x, int y)
        {
            var dbArmy = await _context.Armies.FirstOrDefaultAsync(a => a.Id == armyId);

            if (dbArmy != null)
            {
                dbArmy.X = x;
                dbArmy.Y = y;

                await base.SaveAll();
                return true;
            }

            return false;
        }
    }
}
