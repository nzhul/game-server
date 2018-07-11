using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Heroes;
using Server.Models.Pagination;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;

namespace Server.Data.Services.Implementation
{
    public class RealmsService : BaseService, IRealmsService
    {
        public RealmsService(DataContext context)
            : base(context)
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

            if (avatars != null)
            {
                avatars.Heroes = avatars.Heroes.OrderByDescending(h => h.LastActivity).ToList();
            }

            return avatars;
        }

        public async Task<Avatar> CreateHeroOrAvatarWithHero(int realmId, int userId, AvatarWithHeroCreationDto input)
        {
            Realm dbRealm = await _context.Realms
                .Include(r => r.Regions)
                .FirstOrDefaultAsync(r => r.Id == realmId);

            User dbUser = await _context.Users
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (dbRealm != null && dbUser != null)
            {
                bool heroNameExists = dbRealm.Avatars.Any(a => a.Heroes.Any(h => h.Name == input.HeroName));
                if (!heroNameExists)
                {
                    Avatar avatarInThisRealm = dbUser.Avatars.FirstOrDefault(a => a.RealmId == realmId);
                    HeroBlueprint blueprint = await _context.HeroBlueprints.FirstOrDefaultAsync(h => h.Class == input.HeroClass);
                    Region region = dbRealm.Regions.FirstOrDefault();

                    if (avatarInThisRealm != null)
                    {
                        // if user has avatar in this realm
                        // add hero to this avatar

                        //TODO: Get heroBlueprint for this hero class and use its data to populate the Hero Fields.

                        // TODO replace this with region level 1.
                        // There must be only 5 regions per realm and each region should have level
                        // The new hero will always start at level 1 region.


                        Hero newHero = CreateHero(input.HeroName, blueprint, region, avatarInThisRealm);

                        avatarInThisRealm.Heroes.Add(newHero);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // else
                        // create avatar and then add hero

                        avatarInThisRealm = new Avatar
                        {
                            Realm = dbRealm,
                            RealmId = dbRealm.Id,
                            User = dbUser,
                            UserId = dbUser.Id
                        };
                        _context.Avatars.Add(avatarInThisRealm);
                        await _context.SaveChangesAsync();

                        Hero newHero = CreateHero(input.HeroName, blueprint, region, avatarInThisRealm);
                        avatarInThisRealm.Heroes.Add(newHero);
                        await _context.SaveChangesAsync();
                    }

                    return avatarInThisRealm;
                }
            }

            return null;
        }

        private Hero CreateHero(string heroName, HeroBlueprint blueprint, Region region, Avatar avatar)
        {
            Hero newHero = new Hero();
            newHero.Name = heroName;
            newHero = Mapper.Map(blueprint, newHero);
            newHero.Region = region;
            newHero.Avatar = avatar;
            newHero.Blueprint = blueprint;

            return newHero;
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
