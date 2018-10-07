using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Server.Models.Heroes;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Data
{
    public static class Seeder
    {
        private static Random r = new Random();
        private static string[] realmNames = new string[]
        {
            "Forstford",
            "Azmar",
            "Hull",
            "Zeffari",
            "Tarnstead",
            //"Wimborne",
            //"Leeside",
            //"Taewe",
            //"Dragomir",
            //"Lanteglos",
            //"Rotherham",
            //"Coombe",
            //"Grimsby",
            //"Sutton",
            //"Aylesbury",
            //"Drumnadrochit",
            //"Wavemeet",
            //"Acton",
            //"Redwater",
            //"Shipton",
        };
        private static string[] regionNames = new string[]
        {
            "Adeljoanland",
            "Principality of Wrong Snouts",
            "Garrickland",
            "Mount Ambassador",
            "Duchy of the Rundown Worms",
            "Aarenland",
            "Duchy of Ambassadors",
            "Worms Sea",
            "Gate Of Bazah",
            "Chelanville",
            "Cute Cobble Lake",
            "Empire of the Misty Horses",
            "Enough Man Realm",
            "Shaky Soldier Forest",
            "Hill Of Ktukcasybar",
            "Miniature Jetty Hills",
            "Mount Count",
            "Realm of the Kegs",
            "Duchy of Amused Women",
            "Lake Anchor",
        };
        private static string[] heroNames = new string[]
        {
            "poisonpaddling",
            "dauphineobject",
            "millecampus",
            "forebittsnobby",
            "starboardstar",
            "lobstercomis",
            "unbalancedkilldeer",
            "teethingtyre",
            "roseateglory",
            "gallowaypassage",
            "broughtdisc",
            "feverspeanut",
            "nitricjoining",
            "twitterfixes",
            "ramenphase",
            "anodetriangulum",
            "residencedunnock",
            "welcomesupporting",
            "nigerianvenus",
            "minercuckoo",
            "wackvapid",
            "radiantspinnaker",
            "syphilisvenues",
            "repulsivescoff",
            "gaysbarmpot",
            "jointscygnus",
            "pacebinomial",
            "conceptjugular",
            "syndromecaspian",
            "squeakamphora",
            "mallardtherapist",
            "clasthornbill",
            "saucypetrified",
            "marksfoamy",
            "macaroonpeacock",
            "valuebrown",
            "teammateplantar",
            "ablationpumice",
            "grilltickets",
            "pastillesvigilant",
            "awakebenthic",
            "parkerdropper",
            "frightencriticize",
            "cameltaut",
            "secondarytangent",
            "isaaccollar"
        };

        public static void Initialize(DataContext _context, UserManager<User> _userManager, RoleManager<Role> _roleManager)
        {
            List<User> users = new List<User>();
            ICollection<Avatar> avatars = new List<Avatar>();
            ICollection<Hero> heroes = new List<Hero>();
            ICollection<HeroBlueprint> heroBlueprints = new List<HeroBlueprint>();

            if (!_context.HeroBlueprints.Any())
            {
                heroBlueprints = Seeder.InitializeHeroBluePrints(_context);
            }

            if (!_context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("SeedData/UserSeedData.json");
                users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Moderator" },
                    new Role { Name = "VIP" },
                    new Role { Name = "User" }
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }

                var adminUser = new User
                {
                    UserName = "nzhul"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("nzhul").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator", "VIP", "User" }).Wait();
                }

                _context.SaveChanges();
            }

            ICollection<Realm> realms = new List<Realm>();

            Array realmTypes = Enum.GetValues(typeof(RealmType));

            if (!_context.Realms.Any())
            {

                for (int i = 0; i < realmNames.Length; i++)
                {
                    Realm newRealm = new Realm();
                    newRealm.Name = realmNames[i];
                    newRealm.ResetDate = GetRandomDate();
                    newRealm.Type = (RealmType)realmTypes.GetValue(r.Next(realmTypes.Length));
                    realms.Add(newRealm);
                    _context.Realms.Add(newRealm);
                    _context.SaveChanges();

                    for (int y = 0; y < 5; y++)
                    {
                        Region newRegion = new Region
                        {
                            Name = regionNames[y],
                            Level = r.Next(1, 5)
                        };

                        newRealm.Regions.Add(newRegion);
                        _context.SaveChanges();
                    }
                }
            }

            if (!_context.Avatars.Any())
            {
                foreach (var user in users)
                {
                    foreach (var realm in realms)
                    {
                        Avatar newAvatar = new Avatar();
                        newAvatar.Wood = r.Next(0, 1000);
                        newAvatar.Ore = r.Next(0, 1000);
                        newAvatar.Gold = r.Next(0, 1000);
                        newAvatar.Gems = r.Next(0, 1000);
                        newAvatar.UserId = user.Id;
                        newAvatar.RealmId = realm.Id;

                        _context.Avatars.Add(newAvatar);
                        avatars.Add(newAvatar);
                    }
                }

                _context.SaveChanges();

                foreach (var avatar in avatars)
                {
                    int iterations = r.Next(2, 5);
                    for (int i = 0; i < iterations; i++)
                    {
                        int regionId = r.Next(1, realmNames.Length * 5);
                        Hero newHero = new Hero
                        {
                            Name = GetRandomHeroName(_context, regionId),
                            LastActivity = GetRandomDate(),
                            TimePlayed = new TimeSpan(r.Next(0, 100), r.Next(0, 23), r.Next(0, 59), r.Next(0, 59)),
                            Level = r.Next(1, 60),
                            BlueprintId = r.Next(1, 6),
                            RegionId = regionId,
                            Attack = r.Next(0, 3),
                            Defence = r.Next(0, 3),
                            PersonalAttack = r.Next(5, 10),
                            PersonalDefense = r.Next(5, 10),
                            Magic = r.Next(0, 3),
                            MagicPower = r.Next(0, 3),
                            Dodge = r.Next(0, 2),
                            Health = r.Next(20, 60),
                            MinDamage = r.Next(7, 12),
                            MaxDamage = r.Next(13, 25),
                            MagicResistance = r.Next(0, 2),
                            Avatar = avatar
                        };

                        avatar.Heroes.Add(newHero);
                        heroes.Add(newHero);
                        _context.SaveChanges();
                    }
                }

            }

        }

        private static ICollection<HeroBlueprint> InitializeHeroBluePrints(DataContext _context)
        {
            ICollection<HeroBlueprint> heroBlueprints = new List<HeroBlueprint>();

            HeroClass[] classes = (HeroClass[])Enum.GetValues(typeof(HeroClass));

            for (int i = 0; i < classes.Length; i++)
            {
                HeroBlueprint bluePrint = new HeroBlueprint
                {
                    Description = Seeder.LoremIpsum(3, 10, 1, 3, 1),
                    PortraitImgUrl = RandomString(20),
                    Class = classes[i],
                    Attack = r.Next(0, 3),
                    Defense = r.Next(0, 3),
                    PersonalAttack = r.Next(5, 10),
                    PersonalDefense = r.Next(5, 10),
                    Magic = r.Next(0, 3),
                    MagicPower = r.Next(0, 3),
                    Dodge = r.Next(0, 2),
                    Health = r.Next(20, 60),
                    MinDamage = r.Next(7, 12),
                    MaxDamage = r.Next(13, 25),
                    MagicResistance = r.Next(0, 2)
                };

                if (i < (float)classes.Length / 2)
                {
                    bluePrint.Faction = HeroFaction.Sanctuary;
                }
                else
                {
                    bluePrint.Faction = HeroFaction.Underworld;
                }

                _context.HeroBlueprints.Add(bluePrint);
                heroBlueprints.Add(bluePrint);
            }

            _context.SaveChanges();
            return heroBlueprints;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static DateTime GetRandomDate()
        {
            DateTime start = new DateTime(2014, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }

        private static string LoremIpsum(int minWords, int maxWords,
            int minSentences, int maxSentences,
            int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
            }

            return result.ToString();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        private static string GetRandomHeroName(DataContext _context, int regionId)
        {
            Region region = _context.Regions.FirstOrDefault(r => r.Id == regionId);

            string heroName = string.Empty;
            bool isFree = false;

            while (!isFree)
            {
                heroName = heroNames[r.Next(1, heroNames.Length)];
                isFree = !region.Realm.Avatars.Any(a => a.Heroes.Any(h => h.Name == heroName));
            }

            return heroName;
        }
    }
}