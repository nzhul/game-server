using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Data.Services.Abstraction;
using Server.Models.MapEntities;

namespace Server.Data.Services.Implementation
{
    public class AvatarsService : BaseService, IAvatarsService
    {
        public AvatarsService(DataContext context)
            : base(context)
        {
        }

        public Task<IList<Dwelling>> GetAvatarDwellings(int avatarId)
        {
            throw new System.NotImplementedException();
        }
    }
}
