using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Castles;
using Server.Models.Heroes;

namespace Server.Models.Realms
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public ICollection<Castle> Castles { get; set; }

        public Region()
        {
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
        }

        // public ICollection<MonsterCamp> MonsterCamps { get; set; }

        // public ICollection<Interactable> Interactables { get; set; }

        // public ICollection<Mine> Mines { get; set; }
    }
}