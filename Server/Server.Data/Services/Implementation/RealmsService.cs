using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Pagination;
using Server.Models.Realms;
using Server.Models.Users;

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
                .Include(r => r.Avatars).ThenInclude(c => c.Heroes)
                .Include(r => r.Regions).ThenInclude(c => c.Castles)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // TODO this switch statement can be refactored and switcher with OData.
        public async Task<PagedList<Realm>> GetRealms(QueryParams queryParams)
        {
            IQueryable<Realm> realms = _context.Realms.Include(x => x.Avatars).AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.OrderBy))
            {
                switch (queryParams.OrderBy)
                {
                    case "Population":
                        switch (queryParams.OrderDirection)
                        {
                            case OrderDirection.Ascending:
                                realms = realms.OrderBy(r => r.Avatars.Count);
                                break;
                                
                            case OrderDirection.Descending:
                                realms = realms.OrderByDescending(r => r.Avatars.Count);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;

                    case "Name":
                        switch (queryParams.OrderDirection)
                        {
                            case OrderDirection.Ascending:
                                realms = realms.OrderBy(r => r.Name);
                                break;
                                
                            case OrderDirection.Descending:
                                realms = realms.OrderByDescending(r => r.Name);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;

                    case "Type":
                        switch (queryParams.OrderDirection)
                        {
                            case OrderDirection.Ascending:
                                realms = realms.OrderBy(r => r.Type);
                                break;

                            case OrderDirection.Descending:
                                realms = realms.OrderByDescending(r => r.Type);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;

                    case "ResetDate":
                        switch (queryParams.OrderDirection)
                        {
                            case OrderDirection.Ascending:
                                realms = realms.OrderBy(r => r.ResetDate);
                                break;

                            case OrderDirection.Descending:
                                realms = realms.OrderByDescending(r => r.ResetDate);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                }
            }

            return await PagedList<Realm>.CreateAsync(realms, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<Avatar> GetUserAvatarForRealm(int realmId, int userId)
        {
            var avatars = await _context.Avatars
                .Include(a => a.Heroes)
                .ThenInclude(x => x.Blueprint)
                .FirstOrDefaultAsync(a => a.RealmId == realmId && a.UserId == userId);

            // note: you cannot orderBy the heroes while queryng.
            avatars.Heroes = avatars.Heroes.OrderByDescending(h => h.LastActivity).ToList();

            return avatars;
        }

        public async Task UpdateCurrentRealm(int userId, int realmId)
        {
            User dbUser = await _context.Users.FirstOrDefaultAsync(r => r.Id == userId);

            if (dbUser != null)
            {
                dbUser.CurrentRealmId = realmId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
