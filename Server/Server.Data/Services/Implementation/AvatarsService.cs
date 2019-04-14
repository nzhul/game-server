using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Data.Services.Implementation
{
    public class AvatarsService : BaseService, IAvatarsService
    {
        public AvatarsService(DataContext context)
            : base(context)
        {
        }

        public async Task<IList<Dwelling>> Explore(int avatarId, DwellingType dwellingType, int[] regionIds)
        {
            IList<Dwelling> exploredDwellings = new List<Dwelling>();
            Avatar dbAvatar = await _context.Avatars.Include(a => a.AvatarDwellings).FirstOrDefaultAsync(a => a.Id == avatarId);
            IList<Region> regionsToExplore;

            if (dbAvatar != null)
            {
                if (regionIds != null && regionIds.Length > 0)
                {
                    regionsToExplore = await _context.Regions
                        .Where(r => r.RealmId == dbAvatar.RealmId)
                        .Where(r => regionIds.Contains(r.Id))
                        .Include(r => r.Dwellings)
                        .ToListAsync();
                }
                else
                {
                    regionsToExplore = await _context.Regions
                        .Where(r => r.RealmId == dbAvatar.RealmId)
                        .Include(r => r.Dwellings)
                        .ToListAsync();
                }

                foreach (var region in regionsToExplore)
                {
                    this.AssignDwelling(dbAvatar, region, dwellingType, false);
                }

                await _context.SaveChangesAsync();
            }

            return exploredDwellings;
        }

        private void AssignDwelling(Avatar avatar, Region region, DwellingType type, bool isOwner)
        {
            var dwellings = region.Dwellings.Where(d => d.Type == type);

            foreach (var dwelling in dwellings)
            {
                if (!avatar.AvatarDwellings.Any(ad => ad.AvatarId == avatar.Id && ad.DwellingId == dwelling.Id))
                {
                    avatar.AvatarDwellings.Add(new AvatarDwelling { Avatar = avatar, Dwelling = dwelling });
                }

                if (isOwner)
                {
                    dwelling.Owner = avatar;
                    dwelling.OwnerId = avatar.Id;
                }
            }
        }

        public Task<IList<Dwelling>> GetAvatarDwellings(int avatarId)
        {
            throw new System.NotImplementedException();
        }
    }
}
