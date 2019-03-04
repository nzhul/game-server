using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.MapEntities;

namespace Server.Data.Services.Abstraction
{
    public interface IAvatarsService : IService
    {
        Task<IList<Dwelling>> GetAvatarDwellings(int avatarId);
    }
}