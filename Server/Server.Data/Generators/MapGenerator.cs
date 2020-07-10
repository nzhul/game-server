using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Server.Data.Generators.Models;
using Server.Models;
using Server.Models.Armies;
using Server.Models.Heroes;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;

namespace Server.Data.Generators
{
    /// <summary>
    /// Sizes:
    /// 36x36
    /// 72x72
    /// 108x108
    /// 144x144
    /// </summary>
    public class MapGenerator : IMapGenerator
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public int[,] Matrix { get; private set; }

        /// <summary>
        /// 0 - walkable
        /// 1 - wall -> not walkable
        /// 2 - contact point -> can be reached if it is destination target, but cannot be part of the path. Eg: treasure on the map or Entrace of a castle.
        /// 3 -> occupied -> not walkable -> Additional space of a casle that is not entrance.
        /// </summary>
        public int[,] PopulatedMatrix { get; private set; }

        //public IList<Map> Zones { get; set; }

        private List<TempRoom> PopulationRooms;

        private List<Coord> TempRoomTiles;

        private List<Coord> TempEdgeRoomTiles;

        private Random rand;

        private bool MapIsFullForDwellings;

        private bool MapIsFullForMonstersOrTreasure;

        public int PassageRadius { get; private set; }

        public string Seed { get; set; }

        public IDictionary<MapTemplate, MapConfig> Templates { get; set; }

        public MapConfig Template { get; set; }

        public MapGenerator(string seed)
        {
            this.Seed = seed;
        }

        public MapGenerator()
        {
            this.Templates = this.LoadTemplates(); // TODO: extract this in static singleton so we dont have to load them every time.
        }

        public Map TryGenerateMap(GameParams gameParams)
        {
            this.Template = this.Templates[gameParams.MapTemplate];
            var zones = new List<Map>();

            foreach (var zoneConfig in this.Template.Zones)
            {
                var zone = this.TryGenerateZone(zoneConfig);
                zone.Position = new Coord(zoneConfig.X, zoneConfig.Y);
                zones.Add(zone);
            }

            var map = this.GenerateEmptyMap(this.Template.Height, this.Template.Width);
            this.Width = this.Template.Width;
            this.Height = this.Template.Height;
            this.MergeZones(map, zones, this.Template.Zones);

            // 1. load template config
            // 2. generate zones using template config, mapsize and mapdifficulty -> use default values for now.
            // 3. merge zones into one big map.
            // 4. Connect zones.
            // 5. Assign players at random starting positions from the availible.
            // 6. return the map.

            return map;
        }

        private void MergeZones(Map map, List<Map> zones, IList<ZoneConfig> zoneConfigs)
        {
            map.Rooms = new List<TempRoom>();

            for (int i = 0; i < zones.Count; i++)
            {
                var zone = zones[i];
                var config = zoneConfigs[i];
                var yShift = 0 + config.Y;
                var xShift = 0 + config.X;

                var height = zone.Matrix.GetLength(0); // rows
                var width = zone.Matrix.GetLength(1); // cols

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var zoneTileValue = zone.Matrix[y, x];
                        map.Matrix[y + yShift, x + xShift] = zoneTileValue;
                    }
                }

                // Shift rooms
                foreach (var room in zone.Rooms)
                {
                    var newRoom = this.ShiftRoom(room, yShift, xShift, map.Matrix);
                    newRoom.ZoneIndex = config.Id;
                    map.Rooms.Add(newRoom);
                }

                // Shift dwellings
                foreach (var dwelling in zone.Dwellings)
                {
                    dwelling.X += xShift;
                    dwelling.Y += yShift;
                    dwelling.EndX = dwelling.EndX != 0 ? dwelling.EndX += xShift : 0;
                    dwelling.EndY = dwelling.EndY != 0 ? dwelling.EndY += yShift : 0;
                    if (dwelling.Guardian != null)
                    {
                        dwelling.Guardian.X += xShift;
                        dwelling.Guardian.Y += yShift;
                    }
                    dwelling.OccupiedTilesString = StringifyCoordCollection(this.ApplyPointShift(new Coord(xShift, yShift), dwelling.OccupiedTiles));
                    map.Dwellings.Add(dwelling);
                }

                // Shift heroes
                foreach (var hero in zone.Armies)
                {
                    hero.X += xShift;
                    hero.Y += yShift;
                    map.Armies.Add(hero);
                }

                // Shift treasures
                foreach (var treasure in zone.Treasures)
                {
                    treasure.X += xShift;
                    treasure.Y += yShift;
                    map.Treasures.Add(treasure);
                }
            }



            map.Rooms.Sort();
            map.Rooms.ForEach(r => r.isMainRoom = false);
            map.Rooms.ForEach(r => r.isAccessibleFromMainRoom = false);
            map.Rooms[0].isMainRoom = true;
            map.Rooms[0].isAccessibleFromMainRoom = true;

            this.Matrix = map.Matrix;
            ConnectClosestRooms(map, map.Rooms);
            map.Matrix = this.Matrix;
        }

        private TempRoom ShiftRoom(TempRoom room, int yShift, int xShift, int[,] matrix)
        {
            var shiftedTiles = room.tiles.ToList();
            shiftedTiles.ForEach(t => { t.Y += xShift; t.X += yShift; });

            return new TempRoom(shiftedTiles, matrix);
        }

        public Map TryGenerateZone(ZoneConfig config)
        {
            Map generatedMap = null;

            bool generationIsFailing = true;
            bool retryLimitNotReached = true;
            int retryLimit = 10;
            int currentRetries = 0;

            while (generationIsFailing && retryLimitNotReached)
            {
                try
                {
                    this.Seed = DateTime.UtcNow.Ticks.ToString();
                    generatedMap = this.GenerateZone(config.Width, config.Height, randomFillPercent: config.Fuzziness);
                    generatedMap = this.PopulateZone(generatedMap, config.Team, 50, 50);
                    generationIsFailing = false;
                }
                catch (Exception ex)
                {
                    if (currentRetries <= retryLimit)
                    {
                        currentRetries++;
                    }
                    else
                    {
                        retryLimitNotReached = false;
                        throw ex;
                    }
                }
            }

            return generatedMap;
        }

        public Map GenerateZone(int width = 128,
            int height = 76,
            int borderSize = 0,
            int passageRadius = 1,
            int minRoomSize = 10,
            int minWallSize = 10,
            int randomFillPercent = 47)
        {
            this.Width = width;
            this.Height = height;
            this.Matrix = new int[width, height];
            this.PassageRadius = passageRadius;

            Map map = new Map();
            List<TempRoom> rooms = new List<TempRoom>();

            //!!!TODO: Instead of filling the map randomly.
            // I can try to initialize the matrix with given hardcoded areas and then 
            // run the rest of the generation logic
            // This should cause the map the be proceduraly generated but to have a recognizable pattern
            // ■■■■■■■■■■■■■■■■■■■■
            // ■■■■■□□□■■■■■□□□■■■■
            // ■■■■■□□□■■■■■□□□■■■■
            // ■■■■■■■■■■■■■■■■■■■■
            // ■■■■■□□□■■■■■□□□■■■■
            // ■■■■■□□□■■■■■□□□■■■■
            // ■■■■■■■■■■■■■■■■■■■■

            this.RandomFillMap(width, height, randomFillPercent);

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }

            rooms = ProcessMap(map, minWallSize, minRoomSize);

            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

            for (int x = 0; x < borderedMap.GetLength(0); x++)
            {
                for (int y = 0; y < borderedMap.GetLength(1); y++)
                {
                    if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                    {
                        borderedMap[x, y] = this.Matrix[x - borderSize, y - borderSize];
                    }
                    else
                    {
                        borderedMap[x, y] = 1;
                    }
                }
            }

            this.Matrix = borderedMap;
            map.Matrix = this.Matrix;
            map.Rooms = rooms;
            map.Seed = this.Seed;
            return map;
        }

        private List<Room> TransformRoomEntities(List<TempRoom> rooms)
        {
            List<Room> dbRooms = new List<Room>();

            foreach (var room in rooms)
            {
                Room newRoom = new Room
                {
                    RoomSize = room.roomSize,
                    IsMainRoom = room.isMainRoom,
                    IsAccessibleFromMainRoom = room.isAccessibleFromMainRoom,
                    TilesString = StringifyCoordCollection(room.tiles),
                    EdgeTilesString = StringifyCoordCollection(room.edgeTiles)
                };
                dbRooms.Add(newRoom);
            }

            return dbRooms;
        }

        /// <summary>
        /// Returns a collection of survivingRooms for the map
        /// </summary>
        private List<TempRoom> ProcessMap(Map map, int minWallSize, int minRoomSize)
        {
            List<List<Coord>> wallRegions = GetRegions(1);

            foreach (List<Coord> wallRegion in wallRegions)
            {
                if (wallRegion.Count < minWallSize)
                {
                    foreach (Coord tile in wallRegion)
                    {
                        this.Matrix[tile.X, tile.Y] = 0;
                    }
                }
            }

            List<List<Coord>> roomRegions = GetRegions(0);

            List<TempRoom> survivingRooms = new List<TempRoom>();

            foreach (List<Coord> roomRegion in roomRegions)
            {
                if (roomRegion.Count < minRoomSize)
                {
                    foreach (Coord tile in roomRegion)
                    {
                        this.Matrix[tile.X, tile.Y] = 1;
                    }
                }
                else
                {
                    survivingRooms.Add(new TempRoom(roomRegion, this.Matrix));
                }
            }

            survivingRooms.Sort();

            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;

            ConnectClosestRooms(map, survivingRooms);

            return survivingRooms;
        }

        private void ConnectClosestRooms(Map map, List<TempRoom> allRooms, bool forceAccessibilityFromMainRoom = false)
        {
            List<TempRoom> roomListA = new List<TempRoom>();
            List<TempRoom> roomListB = new List<TempRoom>();

            if (forceAccessibilityFromMainRoom)
            {
                foreach (TempRoom room in allRooms)
                {
                    if (room.isAccessibleFromMainRoom)
                    {
                        roomListB.Add(room);
                    }
                    else
                    {
                        roomListA.Add(room);
                    }
                }
            }
            else
            {
                roomListA = allRooms;
                roomListB = allRooms;
            }

            int bestDistance = 0;
            Coord bestTileA = new Coord();
            Coord bestTileB = new Coord();
            TempRoom bestRoomA = new TempRoom();
            TempRoom bestRoomB = new TempRoom();
            bool possibleConnectionFound = false;

            foreach (TempRoom roomA in roomListA)
            {
                if (!forceAccessibilityFromMainRoom)
                {
                    possibleConnectionFound = false;
                    if (roomA.connectedRooms.Count > 0)
                    {
                        continue;
                    }
                }

                foreach (TempRoom roomB in roomListB)
                {
                    if (roomA == roomB || roomA.IsConnected(roomB))
                    {
                        continue;
                    }

                    for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                        {
                            Coord tileA = roomA.edgeTiles[tileIndexA];
                            Coord tileB = roomB.edgeTiles[tileIndexB];

                            int distanceBetweenRooms = (int)(Math.Pow(tileA.X - tileB.X, 2) + Math.Pow(tileA.Y - tileB.Y, 2));
                            if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                            {
                                bestDistance = distanceBetweenRooms;
                                possibleConnectionFound = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomA = roomA;
                                bestRoomB = roomB;
                            }
                        }
                    }
                }

                if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
                {
                    CreatePassage(map, bestRoomA, bestRoomB, bestTileA, bestTileB);
                }
            }

            if (possibleConnectionFound && forceAccessibilityFromMainRoom)
            {
                CreatePassage(map, bestRoomA, bestRoomB, bestTileA, bestTileB);
                ConnectClosestRooms(map, allRooms, true);
            }

            if (!forceAccessibilityFromMainRoom)
            {
                ConnectClosestRooms(map, allRooms, true);
            }
        }

        /// <summary>
        /// Connects two rooms with a passage
        /// </summary>
        /// <param name="roomA">First room</param>
        /// <param name="roomB">Second room</param>
        /// <param name="tileA">Closest tile from room A</param>
        /// <param name="tileB">Closest tile from room B</param>
        private void CreatePassage(Map map, TempRoom roomA, TempRoom roomB, Coord tileA, Coord tileB)
        {
            TempRoom.ConnectRooms(roomA, roomB);
            List<Coord> line = GetLine(tileA, tileB);
            var bridgeTiles = new List<Coord>();

            foreach (Coord c in line)
            {
                var circleTiles = DrawCircle(c, this.PassageRadius);
                bridgeTiles.AddRange(circleTiles);
            }

            // ocupiedTilesString = line + DrawCircle coordinates.
            // Invoke CreateDwelling(..., ocupiedTilesString);

            var startCoord = line.First();
            var endCoord = line.Last();

            var zoneLink = this.FindLink(roomA, roomB);
            CreateBridge(map, zoneLink, bridgeTiles, startCoord, endCoord);
        }

        private ZoneLink FindLink(TempRoom roomA, TempRoom roomB)
        {
            var zoneA = this.Template.Zones.FirstOrDefault(z => z.Id == roomA.ZoneIndex);
            var zoneB = this.Template.Zones.FirstOrDefault(z => z.Id == roomB.ZoneIndex);

            if (zoneA != null && zoneB != null)
            {
                var link = zoneA.Links.FirstOrDefault(l => l.ZoneId == zoneB.Id);
                if (link != null)
                {
                    return link;
                }
            }

            return null;
        }

        private List<Coord> DrawCircle(Coord c, int r)
        {
            var tiles = new List<Coord>();

            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int realX = c.X + x;
                        int realY = c.Y + y;

                        if (IsInMapRange(realY, realX))
                        {
                            tiles.Add(new Coord(realX, realY));
                            this.Matrix[realX, realY] = 9;
                        }
                    }
                }
            }

            return tiles;
        }

        private List<Coord> GetLine(Coord from, Coord to)
        {
            List<Coord> line = new List<Coord>();

            int x = from.X;
            int y = from.Y;

            int dx = to.X - from.X;
            int dy = to.Y - from.Y;

            bool inverted = false;

            int step = Math.Sign(dx);
            int gradientStep = Math.Sign(dy);

            int longest = Math.Abs(dx);
            int shortest = Math.Abs(dy);

            if (longest < shortest)
            {
                inverted = true;
                longest = Math.Abs(dy);
                shortest = Math.Abs(dx);

                step = Math.Sign(dy);
                gradientStep = Math.Sign(dx);
            }

            int gradientAccumulation = longest / 2;
            for (int i = 0; i < longest; i++)
            {
                line.Add(new Coord(x, y));

                if (inverted)
                {
                    y += step;
                }
                else
                {
                    x += step;
                }

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                    {
                        x += gradientStep;
                    }
                    else
                    {
                        y += gradientStep;
                    }

                    gradientAccumulation -= longest;
                }
            }

            return line;

        }

        /// <summary>
        /// Returns a list with all regions by type: 
        /// For example: returns a list of rooms "0". Each item in the list is a collection of Coordinates
        /// </summary>
        /// <param name="tileType"></param>
        /// <returns></returns>
        private List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            int[,] mapFlags = new int[this.Width, this.Height];

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (mapFlags[x, y] == 0 && this.Matrix[x, y] == tileType)
                    {
                        List<Coord> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        // mark all tiles in the new region as "looked at" so we dont have overlapping regions.
                        foreach (Coord tile in newRegion)
                        {
                            mapFlags[tile.X, tile.Y] = 1;
                        }
                    }
                }
            }

            return regions;
        }

        /// <summary>
        /// Returns a collection of coordinates for given region
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <returns></returns>
        private List<Coord> GetRegionTiles(int startX, int startY)
        {
            List<Coord> tiles = new List<Coord>();
            int[,] mapFlags = new int[this.Width, this.Height];
            int tileType = this.Matrix[startX, startY];

            Queue<Coord> queue = new Queue<Coord>();
            queue.Enqueue(new Coord(startX, startY));
            mapFlags[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.X - 1; x <= tile.X + 1; x++)
                {
                    for (int y = tile.Y - 1; y <= tile.Y + 1; y++)
                    {
                        if (IsInMapRange(x, y) && (y == tile.Y || x == tile.X))
                        {
                            if (mapFlags[x, y] == 0 && this.Matrix[x, y] == tileType)
                            {
                                mapFlags[x, y] = 1;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }

            return tiles;
        }

        private void SmoothMap()
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                    {
                        this.Matrix[x, y] = 1;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        this.Matrix[x, y] = 0;
                    }
                }
            }
        }

        private int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (IsInMapRange(neighbourX, neighbourY))
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += this.Matrix[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        private bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
        }

        private void RandomFillMap(int width, int height, int randomFillPercent)
        {
            Random r = new Random(this.Seed.GetHashCode());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        this.Matrix[x, y] = 1;
                    }
                    else
                    {
                        this.Matrix[x, y] = (r.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }

                }
            }
        }

        public static string StringifyCoordCollection(List<Coord> coordinates)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var coord in coordinates)
            {
                sb.Append($"{coord.X}:{coord.Y};");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyMap"></param>
        /// <param name="monsterStrength">in percent %</param>
        /// <param name="objectDencity">in percent %</param>
        /// <returns></returns>
        public Map PopulateZone(Map emptyMap, Team team, int monsterStrength, int objectDencity)
        {
            // reset flags
            this.MapIsFullForMonstersOrTreasure = false;
            this.MapIsFullForDwellings = false;

            this.rand = new Random(this.Seed.GetHashCode());
            this.PopulatedMatrix = emptyMap.Matrix;
            this.PopulationRooms = emptyMap.Rooms;
            //this.FreeCellsCount = this.CountFreeCoords(this.PopulationRooms);
            //int minimumFreeCellsLeft = (this.FreeCellsCount * objectDencity) / 100;
            //this.PopulationRooms = this.CalculateInitialFreeCellCountForEveryRoom(this.PopulationRooms);

            // contains map walls + non-walkable cells and interactables like monsters, gold, wood, stone and other
            // At the end we will return to the client an matrix with only 0, 1 and 2.
            // But there will be objects that can be cleared from the map
            // for example a Monster pack can be placed on position -> 21:10.
            // initiali position 21:10 will be marked as 2 (interactable) and not be walkable.
            // when the player clears this position - we will change the value in the matrix to 0
            // and this cell will become walkable

            // TODO: Populate the map with objects
            // 1. Place an object on the map and update the matrix with blocked coordinates
            // 2. Run flood fill algorithm 
            // 3. Validate that there are no isolated regions after the object is placed.
            // 4. If valid -> continue with next placement. Else -> Try to place to new random position
            //
            // try to place buildings and power ups near the edge of the map
            // place resources and consumables at random positions
            // monsters, consumables, resources and other similar objects are not permanent map blockers.
            // this means that they are only temporary blocking the path of the hero. 
            // This is OK and should not mark the objects placement as invalid.
            // NB: fill the map with objects until certain percent is filled. Ex: 70% of all free cells

            //1. Place player hero/castle
            var mainRoom = this.PopulationRooms[0];
            ResetTempVariables(mainRoom);

            Coord castlePosition = this.TryGetSafePosition(mainRoom, PlacementStrategy.FarFromEdge, 4, SpaceRequirements.Dwellings[DwellingType.Castle]);
            this.MarkPositionAsOccupied(castlePosition, SpaceRequirements.Dwellings[DwellingType.Castle], mainRoom);
            var link = Guid.NewGuid();
            CreateDwelling("CastleName", emptyMap, castlePosition, DwellingType.Castle, team, link);
            CreatePlayerArmy(emptyMap, new Coord(castlePosition.X - 1 , castlePosition.Y), team, link);

            // 2. Place waypoint
            PlaceDwelling(emptyMap, "Waypoint", DwellingType.Waypoint, 1, 1);

            // 3. Place wood mines -> min 2 - max 4
            PlaceDwelling(emptyMap, "WoodMine", DwellingType.WoodMine, 2, 3);

            PlaceDwelling(emptyMap, "StoneMine", DwellingType.StoneMine, 2, 3);

            // TODO: don't forget the flood fill alg.
            // -> after getting the object position -> create a copy of PopulationRooms and update it with new object
            // after that -> run flood fill. 
            // If there are more than one (1) separate rooms.
            // discard the position and try again to find safe position.
            // repeat this process untill safe position is found.


            // 4. Place gold mine -> min 0 - max 2
            if (!MapIsFullForDwellings)
            {

            }

            // 5. Place other mines -> min 2 - max 4

            if (!MapIsFullForDwellings)
            {

            }

            // 6. Place monsters

            if (!MapIsFullForMonstersOrTreasure)
            {
                for (int i = 0; i < 100; i++)
                {
                    var availibleRooms = this.PopulationRooms.Where(r => r.AvailibleForMonsterOrTreasurePlacement == true).ToList();
                    if (availibleRooms.Count == 0)
                    {
                        MapIsFullForMonstersOrTreasure = true;
                        break;
                    }

                    var randomRoom = this.GetRandomRoom(availibleRooms); //TODO: keep in mind that there might not be availible rooms and this can fail!
                    this.ResetTempVariables(randomRoom);

                    var monsterType = CreatureType.Spider;
                    var spaceRequired = SpaceRequirements.Monsters[monsterType];
                    Coord monsterPosition = this.TryGetSafePosition(randomRoom, PlacementStrategy.Random, 2, spaceRequired);
                    this.MarkPositionAsOccupied(monsterPosition, spaceRequired, randomRoom);
                    CreateNPCArmy(emptyMap, monsterPosition, monsterType);
                }
            }

            emptyMap.Matrix = this.PopulatedMatrix; // the map is updated with all non-walkable cells
            return emptyMap;
        }

        private void CreatePlayerArmy(Map emptyMap, Coord position, Team team, Guid? link)
        {
            var playerArmy = new Army()
            {
                X = position.Y, // X and Y are switcher because here X is Row. In Unity X is Col
                Y = position.X, // X and Y are switcher because here Y is Col. In Unity Y is Row
                Team = team,
                Link = link,
                NPCData = new NPCData()
            };

            this.AddUnitsToHero(playerArmy);

            emptyMap.Armies.Add(playerArmy);
        }

        private void CreateNPCArmy(Map emptyMap, Coord position, CreatureType type)
        {
            var neutralArmy = new Army()
            {
                X = position.Y, // X and Y are switcher because here X is Row. In Unity X is Col
                Y = position.X ,
                NPCData = new NPCData
                {
                    OccupiedTilesString = StringifyCoordCollection(this.ApplyPointShift(position, SpaceRequirements.Monsters[type])),
                    Disposition = Disposition.Savage,
                    // ItemReward = this.GetRandomLevel1Item(); - call cached version of database for ItemBluePrint with the required level,
                    RewardType = TreasureType.Gold,
                },
                IsNPC = true
            };

            this.AddUnitsToHero(neutralArmy);

            emptyMap.Armies.Add(neutralArmy);
        }

        //private Hero CreateHero()
        //{

        //}

        private void AddUnitsToHero(Army neutralArmy)
        {
            var heroUnit = new Unit
            {
                Type = CreatureType.Knight,
                Level = 1,
                Quantity = 1,
                StartX = 0,
                StartY = 0
            };

            var unit1 = new Unit
            {
                Quantity = 1,
                Type = CreatureType.Troll,
                StartX = 0,
                StartY = 2
            };

            var unit2 = new Unit
            {
                Quantity = 2,
                Type = CreatureType.Troll,
                StartX = 0,
                StartY = 4
            };

            var unit3 = new Unit
            {
                Quantity = 2,
                Type = CreatureType.Shaman,
                StartX = 0,
                StartY = 8
            };

            neutralArmy.Units.Add(heroUnit);
            neutralArmy.Units.Add(unit1);
            neutralArmy.Units.Add(unit2);
            neutralArmy.Units.Add(unit3);
        }

        private void PlaceDwelling(Map emptyMap, string name, DwellingType type, int minCount, int maxCount)
        {
            int woodMinesCount = rand.Next(minCount, maxCount + 1);
            for (int i = 0; i < woodMinesCount; i++)
            {
                var availibleRooms = this.PopulationRooms.Where(r => r.AvailibleForDwellingPlacement == true).ToList();
                if (availibleRooms.Count == 0)
                {
                    MapIsFullForDwellings = true;
                    break;
                }

                TempRoom randomRoom = this.GetRandomRoom(availibleRooms);
                this.ResetTempVariables(randomRoom);

                var spaceRequirement = SpaceRequirements.Dwellings[type];
                Coord dwellingPosition = this.TryGetSafePosition(randomRoom, PlacementStrategy.Random, 2, spaceRequirement);
                this.MarkPositionAsOccupied(dwellingPosition, spaceRequirement, randomRoom);
                CreateDwelling(name, emptyMap, dwellingPosition, type);
            }
        }

        private void CreateBridge(Map map, ZoneLink zoneLink, List<Coord> tiles, Coord start, Coord end)
        {
            // TODO: if zoneLink is null - use default values

            Army newGuardian = null;

            if (zoneLink != null)
            {
                newGuardian = new Army()
                {
                    //Level = zoneLink.GuardianStrength, // TODO: use the GuardianStrenght
                    X = start.X,
                    Y = start.Y,
                    NPCData = new NPCData
                    {
                        Disposition = Disposition.Savage,
                        // ItemReward = this.GetRandomLevel1Item(); - call cached version of database for ItemBluePrint with the required level,
                        RewardType = TreasureType.Gold,
                    },
                    IsNPC = true
                };

                this.AddUnitsToHero(newGuardian);
            }

            Dwelling newDwelling = new Dwelling()
            {
                Type = DwellingType.Bridge,
                X = start.Y, // X and Y are switcher because here X is Row. In Unity X is Col
                Y = start.X,
                EndX = end.Y, // X and Y are switcher because here X is Row. In Unity X is Col
                EndY = end.X,
                Guardian = newGuardian,
                //OwnerId = 1, //TODO: pass this as parameter in MapGeneration function.
                OccupiedTilesString = StringifyCoordCollection(tiles),
            };

            map.Dwellings.Add(newDwelling);
        }

        private void CreateDwelling(string name, Map emptyMap, Coord position, DwellingType type, Team team = Team.Neutral, Guid? link = null)
        {
            Dwelling dwelling = new Dwelling()
            {
                Type = type,
                X = position.Y, // X and Y are switcher because here X is Row. In Unity X is Col
                Y = position.X, 
                //OwnerId = 1, //TODO: pass this as parameter in MapGeneration function.
                OccupiedTilesString = StringifyCoordCollection(this.ApplyPointShift(position, SpaceRequirements.Dwellings[type])),
                Team = team,
                Link = link
            };

            emptyMap.Dwellings.Add(dwelling);
        }

        private void MarkPositionAsOccupied(Coord safePosition, List<Coord> additionalRequiredSpace, TempRoom room)
        {
            foreach (Coord offset in additionalRequiredSpace)
            {
                Coord updatedCoord = new Coord(safePosition.X + offset.X, safePosition.Y + offset.Y);
                this.PopulatedMatrix[updatedCoord.X, updatedCoord.Y] = 3;
                room.FreeCellsLeft--;
            }
            this.PopulatedMatrix[safePosition.X, safePosition.Y] = 2;
            room.FreeCellsLeft--;
        }

        private Coord TryGetSafePosition(TempRoom room, PlacementStrategy strategy, int edgeDistance, List<Coord> additionalRequiredSpace)
        {
            Coord safePosition = null;
            int retriesCount = 0;

            while (safePosition == null)
            {
                safePosition = this.GetRandomSafePosition(room, strategy, edgeDistance, additionalRequiredSpace);

                if (retriesCount > 10 && retriesCount < 60)
                {
                    strategy = PlacementStrategy.Random;
                }

                if (retriesCount > 60)
                {
                    // TODO: think about that happens when this exception is thrown
                    // 1. Fail the map generation and return an error
                    // 2. Ignore this object placement and stop the population process. This might result half-populated map
                    // 3. Ignore this object placement and continue with next one.
                    // 4. Completely scrap the current map generation and start new map generation process.
                    throw new Exception("Cannot find free space for object ...");
                }

                retriesCount++;
            }

            return safePosition;
        }

        private void ResetTempVariables(TempRoom room)
        {
            this.TempRoomTiles = new List<Coord>(room.tiles);
            this.TempEdgeRoomTiles = new List<Coord>(room.edgeTiles);
        }

        private List<Coord> ApplyPointShift(Coord position, List<Coord> list)
        {
            List<Coord> updatedList = new List<Coord>();

            foreach (var item in list)
            {
                updatedList.Add(new Coord(position.Y + item.Y, position.X + item.X));
            }

            return updatedList;
        }

        private int CountFreeCoords(List<Room> populationRooms)
        {
            int count = 0;

            foreach (var room in populationRooms)
            {
                count += room.Tiles.Count;
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room">Room where the object will be placed</param>
        /// <param name="placementStrategy">Placement strategy: Far from edge, Near edge or random</param>
        /// <param name="edgeDistance">Distance to the nearest edge or other object</param>
        /// <param name="additionalRequiredSpace">Used when object require more than once cell space to be placed</param>
        /// <returns>Cotanct point</returns>
        private Coord GetRandomSafePosition(TempRoom room, PlacementStrategy placementStrategy, int edgeDistance, List<Coord> additionalRequiredSpace)
        {
            Coord position = room.tiles[rand.Next(room.tiles.Count)];
            bool positionIsSafe = false;

            switch (placementStrategy)
            {
                case PlacementStrategy.Random:
                    if (!this.IsOnEdge(this.TempEdgeRoomTiles, position) && !IsColliding(position, additionalRequiredSpace))
                    {
                        positionIsSafe = true;
                    }
                    break;
                case PlacementStrategy.NearEdge:
                    break;
                case PlacementStrategy.FarFromEdge:
                    if (IsFarFromEdge(position, edgeDistance, CheckDirection.All) && !IsColliding(position, additionalRequiredSpace))
                    {
                        positionIsSafe = true;
                    }
                    break;
                default:
                    break;
            }

            if (positionIsSafe)
            {
                this.TempRoomTiles.RemoveAll(t => t.X == position.X && t.Y == position.Y);
                return position;
            }

            return null;

            // !!! to reduce the iterations for getting random position.
            // we should mark tested position as invalid for this particular object
            // this way we won't try multiple times to place the object on invalid position.
            // !!! remember to remove the coordinate from availible for placement room tiles

            // if PlacementStrategy.Random
            // 1. get random position
            // 2. if the object is outside of the map -> shift it with nesesary offset so it is inside the map
            // and it is not colliding with existing objects
            // 3. run flood fill to validate if the object is not blocking movement
            // 4. on fail -> repeat
            // 5. on success -> continue


            // if PlacementStrategy.FarFromEdge
            // 1. get random room position that is far from edge ( use edgeDistance ).
            // 2. check if the object is colliding with other objects
            // 3. run flood fill to validate if the object is not blocking movement
            // 4. on fail -> repeat
            // 5. on success -> continue


            // if PlacementStrategy.NearEdge
            // 1. Get random position from edgeTiles
            // 2. shift the object with nesesary offset based on its position so it is inside the map
            //  * shifting the object can happen with "shooting rays" in four directions -> top, right, bottom, left.
            //  * by doing this we can find the longest ray and based on that decide in what direction we can move the object.
            // and it is not colliding with existing objects
            // 3. run flood fill to validate if the object is not blocking movement
            // 4. on fail -> repeat
            // 5. on success -> continue
        }

        private bool IsFarFromEdge(Coord randomRoomPosition, int edgeDistance, CheckDirection all)
        {
            bool result = false;

            if (this.IsOnEdge(this.TempEdgeRoomTiles, randomRoomPosition))
            {
                return false;
            }

            switch (all)
            {
                case CheckDirection.All:
                    int nDist = this.GetDistanceFromEdge(randomRoomPosition, CheckDirection.North);
                    int eDist = this.GetDistanceFromEdge(randomRoomPosition, CheckDirection.East);
                    int sDist = this.GetDistanceFromEdge(randomRoomPosition, CheckDirection.South);
                    int wDist = this.GetDistanceFromEdge(randomRoomPosition, CheckDirection.West);

                    if (nDist > edgeDistance && eDist > edgeDistance && sDist > edgeDistance && wDist > edgeDistance)
                    {
                        result = true;
                    }

                    break;
                case CheckDirection.North:
                    break;
                case CheckDirection.East:
                    break;
                case CheckDirection.South:
                    break;
                case CheckDirection.West:
                    break;
                default:
                    break;
            }

            return result;
        }

        private int GetDistanceFromEdge(Coord coord, CheckDirection direction)
        {
            Coord shiftingCoord = new Coord(coord.X, coord.Y);

            int XOffset = 0;
            int YOffset = 0;
            switch (direction)
            {
                case CheckDirection.All:
                    break;
                case CheckDirection.North:
                    YOffset += 1;
                    break;
                case CheckDirection.East:
                    XOffset -= 1;
                    break;
                case CheckDirection.South:
                    YOffset -= 1;
                    break;
                case CheckDirection.West:
                    XOffset += 1;
                    break;
                default:
                    break;
            }

            int distance = 0;
            bool edgeReached = false;
            while (!edgeReached)
            {
                if (IsOnEdge(this.TempEdgeRoomTiles, shiftingCoord))
                {
                    edgeReached = true;
                }
                else
                {
                    shiftingCoord.X += XOffset;
                    shiftingCoord.Y += YOffset;
                    distance++;
                }
            }

            return distance;
        }

        private bool IsOnEdge(List<Coord> edges, Coord coord)
        {
            return edges.Any(t => t.X == coord.X && t.Y == coord.Y);
        }

        //TODO: test if this is working!
        private bool IsColliding(Coord coord, List<Coord> additionalRequiredSpace)
        {
            bool colliding = false;

            if (this.PopulatedMatrix[coord.X, coord.Y] != 0)
            {
                colliding = true;
            }

            foreach (Coord offset in additionalRequiredSpace)
            {
                Coord updatedCoord = new Coord(coord.X + offset.X, coord.Y + offset.Y);
                if (this.PopulatedMatrix[updatedCoord.X, updatedCoord.Y] != 0)
                {
                    colliding = true;
                }
            }

            return colliding;
        }

        private TempRoom GetRandomRoom(List<TempRoom> rooms)
        {
            return rooms[rand.Next(rooms.Count)];
        }

        private enum PlacementStrategy
        {
            Random = 0,
            NearEdge = 1,
            FarFromEdge = 2
        }

        private IDictionary<MapTemplate, MapConfig> LoadTemplates()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Generators\Templates\");
            var templateFiles = Directory.GetFiles(path);

            var templates = new Dictionary<MapTemplate, MapConfig>();

            foreach (var file in templateFiles)
            {
                var config = JsonConvert.DeserializeObject<MapConfig>(File.ReadAllText(file));
                templates.Add(config.MapTemplate, config);
            }

            return templates;
        }

        private Map GenerateEmptyMap(int rowLen, int colLen)
        {
            var map = new Map();
            map.Matrix = new int[rowLen, colLen];

            for (int row = 0; row < rowLen; row++)
            {
                for (int col = 0; col < colLen; col++)
                {
                    map.Matrix[row, col] = 1;
                }
            }

            return map;
        }
    }

    public class TempRoom : IComparable<TempRoom>
    {
        public int ZoneIndex { get; set; }

        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<TempRoom> connectedRooms;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        private int _roomSize;

        public int roomSize
        {
            get
            {
                return this._roomSize;
            }
            set
            {
                this._roomSize = value;
                this.FreeCellsLeft = value;
                this._minimumFreeCellsRequirementDwellings = (this.roomSize * freePercentDwellings) / 100;
                this._minimumFreeCellsRequirementMonstersAndTreasure = (this.roomSize * freePercentMonstersAndTreasure) / 100;
            }
        }

        public int FreeCellsLeft { get; set; }

        private readonly int freePercentDwellings = 95; // TODO: extract this in configuration

        // TODO: extract this in configuration => was 91
        // TODO: REFACTOR THIS:
        // I need to refactor monster placement logic.
        // I should have something like that:
        // Density: 10-20
        private readonly int freePercentMonstersAndTreasure = 94;


        private int _minimumFreeCellsRequirementDwellings;


        private int _minimumFreeCellsRequirementMonstersAndTreasure;


        public bool AvailibleForDwellingPlacement
        {
            get
            {
                return this.FreeCellsLeft > this._minimumFreeCellsRequirementDwellings;
            }
        }

        public bool AvailibleForMonsterOrTreasurePlacement
        {
            get
            {
                return this.FreeCellsLeft > this._minimumFreeCellsRequirementMonstersAndTreasure;
            }
        }

        public TempRoom()
        {
        }

        public TempRoom(List<Coord> tiles, int[,] map)
        {
            this.tiles = tiles;
            this.roomSize = tiles.Count;
            connectedRooms = new List<TempRoom>();
            edgeTiles = new List<Coord>();

            foreach (Coord tile in tiles)
            {
                for (int x = tile.X - 1; x <= tile.X + 1; x++)
                {
                    for (int y = tile.Y - 1; y <= tile.Y + 1; y++)
                    {
                        if (x == tile.X || y == tile.Y)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (TempRoom connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(TempRoom roomA, TempRoom roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }

            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public int CompareTo(TempRoom other)
        {
            return other.roomSize.CompareTo(this.roomSize);
        }

        public bool IsConnected(TempRoom otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }
    }
}
