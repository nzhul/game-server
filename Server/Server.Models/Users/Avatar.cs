using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Castles;
using Server.Models.Heroes;
using Server.Models.Realms;

namespace Server.Models.Users
{
    /// <summary>
    /// Avatar is the user representation in the current world.
    /// The user can have multiple avatars, but only one avatar per realm
    /// </summary>
    public class Avatar : Entity
    {
        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public virtual ICollection<Hero> Heroes { get; set; }

        public virtual ICollection<Castle> Castles { get; set; }

        public int RealmId { get; set; }

        public virtual Realm Realm { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public Avatar()
        {
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
        }
    }
}
