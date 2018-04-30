using System.Collections.Generic;

namespace Server.Models.Worlds
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int SeedId { get; set; } // the unique Id of this region that will be used by Unity to build the Region map

        // public ICollection<MonsterCamp> MonsterCamps { get; set; }

        // public ICollection<Interactable> Interactables { get; set; }

        // public ICollection<Mine> Mines { get; set; }
    }
}