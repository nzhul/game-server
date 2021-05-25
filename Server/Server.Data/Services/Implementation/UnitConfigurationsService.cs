using Server.Data.Services.Abstraction;
using Server.Models.UnitConfigurations;
using Server.Models.MapEntities;
using System.Linq;

namespace Server.Data.Services.Implementation
{
    public class UnitConfigurationsService : BaseService, IUnitConfigurationsService
    {
        public UnitConfigurationsService(DataContext context)
            : base(context)
        {
        }

        public IQueryable<UnitConfiguration> GetConfigurations(CreatureType? creatureType)
        {
            var dbConfigQuery = _context.UnitConfigurations.AsQueryable();

            if (creatureType.HasValue)
            {
                dbConfigQuery = dbConfigQuery.Where(x => x.Type == creatureType);
            }

            return dbConfigQuery;
        }
    }
}
