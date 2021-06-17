using System;

namespace GameServer.Models.Users
{
    public class User
    {
        private string _created = null;
        private string _lastActive = null;

        public int Id { get; set; }
        public string Username { get; set; }
        public int Mmr { get; set; }
        public int CurrentRealmId { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public string Token { get; set; }
        public Avatar Avatar { get; set; }
        public ServerConnection Connection { get; set; }


        public DateTime? DateCreated
        {
            get { return Convert.ToDateTime(_created); }
        }

        public DateTime? DateLastActive
        {
            get { return Convert.ToDateTime(_lastActive); }
        }

    }
}
