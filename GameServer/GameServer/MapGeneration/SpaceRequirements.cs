﻿using System.Collections.Generic;
using GameServer.Models;
using NetworkingShared.Enums;
using NetworkShared.Models;

namespace GameServer.MapGeneration
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
                new Coord { X = 2, Y = -1 },
                new Coord { X = 2, Y = 0 },
                new Coord { X = 2, Y = 1 },

                new Coord { X = 1, Y = -2 },
                new Coord { X = 1, Y = -1 },
                new Coord { X = 1, Y = 0 },
                new Coord { X = 1, Y = 1 },
                new Coord { X = 1, Y = 2 },

                new Coord { X = 0, Y = -2 },
                new Coord { X = 0, Y = -1 },
                new Coord { X = 0, Y = 0 },
                new Coord { X = 0, Y = 1 },
                new Coord { X = 0, Y = 2 },
            });

            // □□□
            // □■□
            Dwellings.Add(DwellingType.WoodMine, new List<Coord>()
            {
                new Coord { X = 1, Y = -1 },
                new Coord { X = 1, Y = 0 },
                new Coord { X = 1, Y = 1 },

                new Coord { X = 0, Y = -1 },
                new Coord { X = 0, Y = 0 },
                new Coord { X = 0, Y = 1 },
            });

            // □□□
            // □■□
            Dwellings.Add(DwellingType.StoneMine, new List<Coord>()
            {
                new Coord { X = 1, Y = -1 },
                new Coord { X = 1, Y = 0 },
                new Coord { X = 1, Y = 1 },

                new Coord { X = 0, Y = -1 },
                new Coord { X = 0, Y = 0 },
                new Coord { X = 0, Y = 1 },
            });

            // ■
            Dwellings.Add(DwellingType.Waypoint, new List<Coord>()
            {
                new Coord { X = 0, Y = 0 }
            });


            // ■
            Monsters.Add(CreatureType.Spider, new List<Coord>()
            {
                new Coord { X = 0, Y = 0 }
            });
        }
        public static Dictionary<DwellingType, List<Coord>> Dwellings { get; set; }

        public static Dictionary<CreatureType, List<Coord>> Monsters { get; set; }
    }
}
