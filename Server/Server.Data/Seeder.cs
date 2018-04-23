using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Server.Models.Users;

namespace Server.Data
{
    public static class Seeder
    {
        public static void Initialize(DataContext _context)
        {
            if (!_context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("SeedData/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    // create the password hash
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSald = passwordSalt;
                    user.Username = user.Username.ToLower();

                    _context.Users.Add(user);
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
    }
}