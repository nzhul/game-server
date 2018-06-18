using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Users;

namespace Server.Models.Realms
{
    public class Realm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Avatar> Avatars { get; set; }

        public ICollection<Region> Regions { get; set; }

        public Realm()
        {
            this.Avatars = new Collection<Avatar>();
            this.Regions = new Collection<Region>();
        }
    }
}