using System.Collections.Generic;
using Server.Models.Users;

namespace Server.Models.Worlds
{
    public class World
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Players {get;set;}

        public ICollection<Region> Regions {get;set;}
    }
}