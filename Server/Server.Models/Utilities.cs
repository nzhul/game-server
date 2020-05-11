using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Server.Models
{
    public static class Utilities
    {
        public const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        public const string USERNAME_AND_DISCRIMINATOR_PATTERN = @"^[a-zA-Z0-9]{4,20}#[0-9]{4}$";
        public const string USERNAME_PATTERN = @"^[a-zA-Z0-9]{4,20}$";
        public const string RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static bool IsEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return Regex.IsMatch(email, EMAIL_PATTERN);
            }
            else
            {
                return false;
            }
        }

        public static bool IsUsernameAndDiscriminator(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return Regex.IsMatch(username, USERNAME_AND_DISCRIMINATOR_PATTERN);
            }
            else
            {
                return false;
            }
        }

        public static bool IsUsername(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return Regex.IsMatch(username, USERNAME_PATTERN);
            }
            else
            {
                return false;
            }
        }

        public static string GenerateRandom(int length)
        {
            Random r = new Random();
            return new string(Enumerable.Repeat(RANDOM_CHARS, length).Select(s => s[r.Next(s.Length)]).ToArray());
        }

        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
