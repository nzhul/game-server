using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Castles;
using Server.Models.Heroes;

namespace Server.Models.Realms
{
    public class Room : Entity
    {
        public ICollection<Hero> Heroes { get; set; }

        public ICollection<Castle> Castles { get; set; }

        /// <summary>
        /// CSV of all tile coordinates:
        /// Ex: 0:0,0:1,0:2, where left is X and right is Y
        /// </summary>
        public string Tiles { get; set; }

        /// <summary>
        /// CSV of all tile coordinates:
        /// Ex: 0:0,0:1,0:2, where left is X and right is Y
        /// </summary>
        public string EdgeTiles { get; set; }
        
        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }

        // public ICollection<MonsterCamp> MonsterCamps { get; set; }

        // public ICollection<Interactable> Interactables { get; set; }

        // public ICollection<Mine> Mines { get; set; }

        public Room()
        {
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
        }
    }
}
