using System.Threading.Tasks;
using Server.Models.Heroes;
using Server.Models.Realms;

namespace Server.Data.Services.Abstraction
{
    public interface IValidationService
    {
        bool IsValidPosition(Region region, int x, int y);
    }
}
