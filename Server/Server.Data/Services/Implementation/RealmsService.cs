using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data.Generators;
using Server.Data.Services.Abstraction;
using Server.Models;
using Server.Models.Heroes;
using Server.Models.Heroes.Units;
using Server.Models.MapEntities;
using Server.Models.Pagination;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Services.Implementation
{
    public class RealmsService : BaseService, IRealmsService
    {
        private readonly IMapper _mapper;

        public RealmsService(DataContext context, IMapper mapper)
            : base(context)
        {
            this._mapper = mapper;
        }

        public async Task<Realm> GetRealm(int id)
        {
            return await _context.Realms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Realm> GetRealmFull(int id)
        {
            return await _context.Realms
                .Include(r => r.Avatars).ThenInclude(c => c.Heroes) // AutoComplete for .ThenInclude is not working!
                .Include(r => r.Regions).ThenInclude(c => c.Rooms) // Do not wonder why you cannot see nested entities :)
                .Include(r => r.Regions).ThenInclude(c => c.Heroes) // Not sure about this duplication ?
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
                .Include(a => a.Heroes).ThenInclude(x => x.Blueprint)
                .Include(a => a.Heroes).ThenInclude(x => x.Units)
                .Include(a => a.AvatarDwellings).ThenInclude(x => x.Dwelling).ThenInclude(x => x.Region)
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
                .ThenInclude(u => u.Heroes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (dbRealm != null && dbUser != null)
            {
                bool heroNameExists = dbRealm.Avatars.Any(a => a.Heroes.Any(h => h.Name == input.HeroName));
                if (!heroNameExists)
                {
                    Avatar avatarInThisRealm = dbUser.Avatars.FirstOrDefault(a => a.RealmId == realmId);
                    HeroBlueprint blueprint = await _context.HeroBlueprints.FirstOrDefaultAsync(h => h.Class == input.HeroClass);

                    // TODO: this line is not tested!
                    // Info: with every new user/avatar created in any realm - we will create new Region that will be "His special starting zone realm"
                    Region region = await this.CreateRegion("R_" + DateTime.UtcNow.Ticks.ToString(), 1, dbRealm);

                    if (avatarInThisRealm == null)
                    {

                        //TODO: Get heroBlueprint for this hero class and use its data to populate the Hero Fields.

                        // TODO replace this with region level 1.
                        // There must be only 5 regions per realm and each region should have level
                        // The new hero will always start at level 1 region.

                        avatarInThisRealm = new Avatar
                        {
                            Realm = dbRealm,
                            RealmId = dbRealm.Id,
                            User = dbUser,
                            UserId = dbUser.Id
                        };
                        _context.Avatars.Add(avatarInThisRealm);
                        await _context.SaveChangesAsync();
                    }

                    // 1. Assign hero to avatar
                    Hero newHero = CreateHero(input.HeroName, blueprint, region, avatarInThisRealm);
                    avatarInThisRealm.Heroes.Add(newHero);

                    // 2. Assign Castle and waypoint to avatar 
                    this.AssignDwelling(avatarInThisRealm, region, DwellingType.Castle, true);
                    this.AssignDwelling(avatarInThisRealm, region, DwellingType.Waypoint, false); // TODO: remove the waypoint assignment!

                    await _context.SaveChangesAsync();

                    return avatarInThisRealm;
                }
            }

            return null;
        }

        private void AssignDwelling(Avatar avatar, Region region, DwellingType type, bool isOwner)
        {
            var dwellings = region.Dwellings.Where(d => d.Type == type);

            foreach (var dwelling in dwellings)
            {
                avatar.AvatarDwellings.Add(new AvatarDwelling { Avatar = avatar, Dwelling = dwelling });

                if (isOwner)
                {
                    dwelling.Owner = avatar;
                    dwelling.OwnerId = avatar.Id;
                }
            }
        }

        private Hero CreateHero(string heroName, HeroBlueprint blueprint, Region region, Avatar avatar)
        {
            // TODO: place the hero on the map
            // use region.MapMatrix to find starting spot coordinates
            // use those coordinates to newHero.X and newHero.Z
            // do the same for player castle.

            Hero newHero = new Hero();
            newHero.X = region.InitialHeroPosition.Row;
            newHero.Y = region.InitialHeroPosition.Col;
            newHero.StartX = 1;
            newHero.StartY = 5;
            newHero.Name = heroName;
            newHero = _mapper.Map(blueprint, newHero);
            newHero.Region = region;
            newHero.Avatar = avatar;
            newHero.Blueprint = blueprint;
            newHero.NPCData = new NPCData();

            this.AddUnitsToHero(newHero);

            return newHero;
        }

        private void AddUnitsToHero(Hero hero)
        {
            var unit1 = new Unit
            {
                Quantity = 1,
                Type = CreatureType.Shaman,
                StartX = 0,
                StartY = 2
            };

            var unit2 = new Unit
            {
                Quantity = 2,
                Type = CreatureType.Troll,
                StartX = 0,
                StartY = 4
            };

            hero.Units.Add(unit1);
            hero.Units.Add(unit2);
        }

        private async Task<Region> CreateRegion(string name, int level, Realm realm)
        {
            int width;
            int height;

            switch (level)
            {
                case 1:
                    width = 50;
                    height = 30;
                    break;
                case 2:
                    width = 70;
                    height = 50;
                    break;
                case 3:
                    width = 100;
                    height = 70;
                    break;
                case 4:
                    width = 140;
                    height = 90;
                    break;
                default:
                    width = 50;
                    height = 30;
                    break;
            }

            Map generatedMap = TryGenerateMap(height, width); // width and height are swapped on purpose.

            Region newRegion = new Region();
            newRegion.Name = name;
            newRegion.Realm = realm;
            newRegion.Level = level;
            newRegion.MatrixString = generatedMap.MatrixString;
            newRegion.Rooms = generatedMap.Rooms;
            newRegion.Dwellings = generatedMap.Dwellings;
            newRegion.Treasures = generatedMap.Treasures;
            newRegion.InitialHeroPosition = generatedMap.InitialHeroPosition;

            foreach (var hero in generatedMap.Heroes)
            {
                newRegion.Heroes.Add(hero);
            }

            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();

            return newRegion;
        }

        private static Map TryGenerateMap(int width, int height)
        {
            IMapGenerator generator = new MapGenerator();
            Map generatedMap = null;

            bool generationIsFailing = true;
            bool retryLimitNotReached = true;
            int retryLimit = 10;
            int currentRetries = 0;

            while (generationIsFailing && retryLimitNotReached)
            {
                try
                {
                    generatedMap = generator.GenerateMap(width, height);
                    generatedMap = generator.PopulateMap(generatedMap, 50, 50);
                    generationIsFailing = false;
                }
                catch (Exception ex)
                {
                    if (currentRetries <= retryLimit)
                    {
                        currentRetries++;
                    }
                    else
                    {
                        retryLimitNotReached = false;
                        throw ex;
                    }
                }
            }

            return generatedMap;
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

        public async Task<Realm> CreateRealm(string name)
        {
            // Create Realm
            Realm newRealm = new Realm();
            newRealm.Name = name;
            newRealm.Type = RealmType.PvE;
            newRealm.ResetDate = DateTime.UtcNow.AddMonths(6);

            _context.Realms.Add(newRealm);
            await _context.SaveChangesAsync();

            // Creating Level 1 regions
            // Level 1 regions will be generated every time a new user is registered.
            int l1Width = 60;
            int l1Height = 40;
            int l1Border = 1;

            // Level 2 regions - count: 18
            int l2Count = 18;
            int l2Width = l1Width * 2;
            int l2Height = l1Height * 2;
            int l2Border = l1Border + 1;
            for (int i = 0; i < l2Count; i++)
            {
                IMapGenerator generator = new MapGenerator();
                // TODO: generator will sometimes throw exceptions when the map is too small
                // wrap the generation in try catch and retry couple of times.
                Map map = generator.GenerateMap(l2Width, l2Height, l2Border, 1, 50, 50, 47);

                Region newRegion = new Region();
                newRegion.MatrixString = this.StringifyMatrix(map.Matrix);
                newRegion.Rooms = map.Rooms;
                newRegion.Level = 2;
                newRegion.Name = "RandomRegionName" + i;

                newRealm.Regions.Add(newRegion);
                await _context.SaveChangesAsync();
            }



            // Level 3 regions - count: 9

            // Level 4 regions - count: 3

            // Level 5 regions - count: 1

            throw new NotImplementedException();
            // 1. Create Realm
            // 2. Populate it with 5 levels of Regions
            //      Level 5 -> 1 Region
            //      Level 4 -> 3 Regions
            //      Level 3 -> 9 Regions (Total active players / X)
            //      Level 2 -> 18 Regions (Total active players / X)
            //      Level 1 -> As many regions as players are in the server. Each player will have it's onw starting region.


            // When registering new player to given Realm i should Extend/Reduce the 

        }

        public IQueryable<Region> GetRegions(int[] regionIds)
        {
            var regions = _context.Regions
                .Include(r => r.Rooms)
                .Include(r => r.Heroes).ThenInclude(r => r.NPCData)
                .Include(r => r.Heroes).ThenInclude(r => r.Units)
                .Include(r => r.Castles)
                .Include(r => r.Treasures)
                .Include(r => r.Dwellings)
                .Where(r => regionIds.Contains(r.Id));

            return regions;
        }

        string StringifyMatrix(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    sb.Append(matrix[x, y].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
