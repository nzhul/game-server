using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
            "Wimborne",
            "Leeside",
            "Taewe",
            "Hull",
            "Lanteglos",
            "Rotherham",
            "Coombe",
            "Grimsby",
            "Sutton",
            "Aylesbury",
            "Drumnadrochit",
            "Wavemeet",
            "Acton",
            "Redwater",
            "Shipton",
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
        private static  string[] avatarNames = new string[]
        {
            "xxSlayerxx"
        };

        public static void Initialize(DataContext _context)
        {
            List<User> users = new List<User>();

            if (!_context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("SeedData/UserSeedData.json");
                users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    // create the password hash
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSald = passwordSalt;
                    user.Username = user.Username.ToLower();
                    user.CurrentRealmId = r.Next(1, realmNames.Length);

                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }

            ICollection<Realm> realms = new List<Realm>();

            Array realmTypes = Enum.GetValues(typeof(RealmType));

            if (!_context.Realms.Any())
            {
                
                foreach (string realmName in realmNames)
                {
                    Realm newRealm = new Realm();
                    newRealm.Name = realmNames[r.Next(0, realmNames.Length)];
                    newRealm.ResetDate = GetRandomDate();
                    newRealm.Type = (RealmType)realmTypes.GetValue(r.Next(realmTypes.Length));
                    realms.Add(newRealm);
                    _context.Realms.Add(newRealm);
                }

                _context.SaveChanges();
            }

            ICollection<Avatar> avatars = new List<Avatar>();

            if (!_context.Avatars.Any())
            {
                for (int i = 0; i < 20; i++)
                {
                    Avatar newAvatar = new Avatar();
                    newAvatar.Wood = r.Next(0, 1000);
                    newAvatar.Ore = r.Next(0, 1000);
                    newAvatar.Gold = r.Next(0, 1000);
                    newAvatar.Gems = r.Next(0, 1000);
                    newAvatar.UserId = r.Next(1, users.Count);
                    newAvatar.RealmId = r.Next(1, realms.Count);

                    _context.Avatars.Add(newAvatar);
                    avatars.Add(newAvatar);
                }

                _context.SaveChanges();
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
    }
}