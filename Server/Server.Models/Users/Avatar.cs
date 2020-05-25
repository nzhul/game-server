using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Castles;
using Server.Models.Heroes;
using Server.Models.MapEntities;
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

        public virtual ICollection<Dwelling> Dwellings { get; set; }

        public int? GameId { get; set; }

        public virtual Game Game { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public Team Team { get; set; }

        public Avatar()
        {
            // TODO: Init wood, ore, gold, gems based on map difficulty.
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
            this.Dwellings = new Collection<Dwelling>();
        }
    }

    public enum Team
    {
        Neutral,
        Team1,
        Team2,
        Team3,
        Team4,
        Team5,
        Team6,
        Team7,
        Team8
    }
}
