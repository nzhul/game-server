using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Pagination;
using Server.Models.Realms;

namespace Server.Data.Services.Implementation
{
    public class RealmsService : BaseService, IRealmsService
    {
        public RealmsService(DataContext context) : base(context)
        {
        }

        public async Task<Realm> GetRealm(int id)
        {
            return await _context.Realms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Realm> GetRealmFull(int id)
        {
            return await _context.Realms
                .Include(r => r.Avatars).ThenInclude( c => c.Heroes)
                .Include(r => r.Regions).ThenInclude(c => c.Castles)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PagedList<Realm>> GetRealms(QueryParams queryParams)
        {
            var realms = _context.Realms.Include(x => x.Avatars).AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                switch (queryParams.OrderBy)
                {
                    case "avatarsCount":
                        realms = realms.OrderByDescending(r => r.Avatars.Count);
                        break;
                }
            }

            return await PagedList<Realm>.CreateAsync(realms, queryParams.PageNumber, queryParams.PageSize);
        }
    }
}
