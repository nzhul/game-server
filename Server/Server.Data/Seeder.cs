using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Server.Models.Heroes;
using Server.Models.Heroes.Units;
using Server.Models.Realms;
using Server.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            ICollection<Unit> heroes = new List<Unit>();
            List<UnitConfiguration> unitConfigurations = new List<UnitConfiguration>();

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

                AddAdminUser(_userManager, "nzhul", "dobromirivanov1@gmail.com");
                AddAdminUser(_userManager, "system", "system@email.com");

                _context.SaveChanges();
            }

            if (!_context.UnitConfigurations.Any())
            {
                var configsData = System.IO.File.ReadAllText("SeedData/UnitConfigurations.json");
                unitConfigurations = JsonConvert.DeserializeObject<List<UnitConfiguration>>(configsData);

                foreach (var config in unitConfigurations)
                {
                    _context.UnitConfigurations.Add(config);
                }

                _context.SaveChanges();
            }
        }

        private static void AddAdminUser(UserManager<User> _userManager, string username, string email)
        {
            var adminUser = new User
            {
                UserName = username,
                Gender = "male",
                Email = email,
                Avatar = new Avatar()
            };

            IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

            if (result.Succeeded)
            {
                var admin = _userManager.FindByNameAsync(username).Result;
                _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator", "VIP", "User" }).Wait();
            }
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

        //private static string GetRandomHeroName(DataContext _context, int regionId)
        //{
        //    Game region = _context.Games.FirstOrDefault(r => r.Id == regionId);

        //    string heroName = string.Empty;
        //    bool isFree = false;

        //    while (!isFree)
        //    {
        //        heroName = heroNames[r.Next(1, heroNames.Length)];
        //        isFree = !region.Realm.Avatars.Any(a => a.Heroes.Any(h => h.Name == heroName));
        //    }

        //    return heroName;
        //}
    }
}