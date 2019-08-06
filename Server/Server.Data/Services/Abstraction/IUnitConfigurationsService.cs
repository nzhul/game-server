using Server.Models.Heroes.Units;
using Server.Models.MapEntities;
using System.Linq;

namespace Server.Data.Services.Abstraction
{
    public interface IUnitConfigurationsService
    {
        IQueryable<UnitConfiguration> GetConfigurations(CreatureType? creatureType);
    }
}
