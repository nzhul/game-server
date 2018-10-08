using System.Collections.Generic;
using Server.Models.Realms;

namespace Server.Data.Generators
{
    public class Map
    {
        public int[,] Matrix { get; set; }

        public List<Room> Rooms { get; set; }

        public string Seed { get; set; }
    }
}
