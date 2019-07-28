using System.Collections.Generic;
using Server.Models;
using Server.Models.MapEntities;

namespace Server.Data.Generators
{
    public static class SpaceRequirements
    {
        static SpaceRequirements()
        {
            Dwellings = new Dictionary<DwellingType, List<Coord>>();
            Monsters = new Dictionary<CreatureType, List<Coord>>();

            //  □□□
            // □□□□□
            // □□■□□
            Dwellings.Add(DwellingType.Castle, new List<Coord>()
            {
                new Coord { Row = 2, Col = -1 },
                new Coord { Row = 2, Col = 0 },
                new Coord { Row = 2, Col = 1 },

                new Coord { Row = 1, Col = -2 },
                new Coord { Row = 1, Col = -1 },
                new Coord { Row = 1, Col = 0 },
                new Coord { Row = 1, Col = 1 },
                new Coord { Row = 1, Col = 2 },

                new Coord { Row = 0, Col = -2 },
                new Coord { Row = 0, Col = -1 },
                new Coord { Row = 0, Col = 0 },
                new Coord { Row = 0, Col = 1 },
                new Coord { Row = 0, Col = 2 },
            });

            // □□□
            // □■□
            Dwellings.Add(DwellingType.WoodMine, new List<Coord>()
            {
                new Coord { Row = 1, Col = -1 },
                new Coord { Row = 1, Col = 0 },
                new Coord { Row = 1, Col = 1 },

                new Coord { Row = 0, Col = -1 },
                new Coord { Row = 0, Col = 0 },
                new Coord { Row = 0, Col = 1 },
            });

            // □□□
            // □■□
            Dwellings.Add(DwellingType.StoneMine, new List<Coord>()
            {
                new Coord { Row = 1, Col = -1 },
                new Coord { Row = 1, Col = 0 },
                new Coord { Row = 1, Col = 1 },

                new Coord { Row = 0, Col = -1 },
                new Coord { Row = 0, Col = 0 },
                new Coord { Row = 0, Col = 1 },
            });

            // ■
            Dwellings.Add(DwellingType.Waypoint, new List<Coord>()
            {
                new Coord { Row = 0, Col = 0 }
            });


            // ■
            Monsters.Add(CreatureType.Spider, new List<Coord>()
            {
                new Coord { Row = 0, Col = 0 }
            });
        }
        public static Dictionary<DwellingType, List<Coord>> Dwellings { get; set; }

        public static Dictionary<CreatureType, List<Coord>> Monsters { get; set; }
    }
}
