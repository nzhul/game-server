using System;

namespace GameServer.Shared.Users
{
    [Serializable]
    public class LoginResponse
    {
        public string tokenString;
        public User user;
    }

    [Serializable]
    public class User
    {
        public int id;
        public string username;
        public int mmr;
        public int currentRealmId;
        public string gender;
        public int age;

        private string created = null;

        private string lastActive = null;

        public string interests;
        public string city;
        public string country;
        public string photoUrl;

        public DateTime? dateCreated
        {
            get { return Convert.ToDateTime(created); }
        }

        public DateTime? dateLastActive
        {
            get { return Convert.ToDateTime(lastActive); }
        }

    }
}
