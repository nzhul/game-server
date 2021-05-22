using System.Collections.Generic;
using GameServer.Models;

namespace GameServer.MapGeneration.Models
{
    public class MapConfig
    {
        public int Width { get; set; } // Width

        public int Height { get; set; } // Height

        public MapTemplate MapTemplate { get; set; }

        public IList<ZoneConfig> Zones { get; set; }
    }
}
