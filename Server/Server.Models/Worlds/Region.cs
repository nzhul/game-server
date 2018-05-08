using System.Collections.Generic;
using Server.Models.Heroes;

namespace Server.Models.Worlds
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        // public ICollection<MonsterCamp> MonsterCamps { get; set; }

        // public ICollection<Interactable> Interactables { get; set; }

        // public ICollection<Mine> Mines { get; set; }
    }
}