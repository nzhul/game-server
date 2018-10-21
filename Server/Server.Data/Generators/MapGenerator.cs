﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Models;
using Server.Models.MapEntities;
using Server.Models.Realms;

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

        private List<Room> PopulationRooms;

        private List<Coord> TempRoomTiles;

        private List<Coord> TempEdgeRoomTiles;

        private Random rand;

        private bool MapIsFullForDwellings;

        private bool MapIsFullForMonstersOrTreasure;

        public int PassageRadius { get; private set; }

        public string Seed { get; set; }

        public MapGenerator(string seed)
        {
            this.Seed = seed;
        }

        public MapGenerator()
        {
            this.Seed = DateTime.UtcNow.Ticks.ToString();
        }

        public Map GenerateMap(int width = 128,
            int height = 76,
            int borderSize = 0,
            int passageRadius = 1,
            int minRoomSize = 50,
            int minWallSize = 50,
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

            rooms = ProcessMap(minWallSize, minRoomSize);

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
            map.Rooms = this.TransformRoomEntities(rooms);
            map.Seed = this.Seed;
            return map;
        }

        private List<Room> TransformRoomEntities(List<TempRoom> rooms)
        {
            List<Room> dbRooms = new List<Room>();

            foreach (var room in rooms)
            {
                Room newRoom = new Room();
                newRoom.RoomSize = room.roomSize;
                newRoom.IsMainRoom = room.isMainRoom;
                newRoom.IsAccessibleFromMainRoom = room.isAccessibleFromMainRoom;
                newRoom.TilesString = this.StringifyCoordCollection(room.tiles);
                newRoom.EdgeTilesString = this.StringifyCoordCollection(room.edgeTiles);
                dbRooms.Add(newRoom);
            }

            return dbRooms;
        }

        /// <summary>
        /// Returns a collection of survivingRooms for the map
        /// </summary>
        List<TempRoom> ProcessMap(int minWallSize, int minRoomSize)
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

            ConnectClosestRooms(survivingRooms);

            return survivingRooms;
        }

        void ConnectClosestRooms(List<TempRoom> allRooms, bool forceAccessibilityFromMainRoom = false)
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
                    CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                }
            }

            if (possibleConnectionFound && forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                ConnectClosestRooms(allRooms, true);
            }

            if (!forceAccessibilityFromMainRoom)
            {
                ConnectClosestRooms(allRooms, true);
            }
        }

        /// <summary>
        /// Connects two rooms with a passage
        /// </summary>
        /// <param name="roomA">First room</param>
        /// <param name="roomB">Second room</param>
        /// <param name="tileA">Closest tile from room A</param>
        /// <param name="tileB">Closest tile from room B</param>
        void CreatePassage(TempRoom roomA, TempRoom roomB, Coord tileA, Coord tileB)
        {
            TempRoom.ConnectRooms(roomA, roomB);
            List<Coord> line = GetLine(tileA, tileB);

            foreach (Coord c in line)
            {
                DrawCircle(c, this.PassageRadius);
            }
        }

        void DrawCircle(Coord c, int r)
        {
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int realX = c.X + x;
                        int realY = c.Y + y;

                        if (IsInMapRange(realX, realY))
                        {
                            this.Matrix[realX, realY] = 0;
                        }
                    }
                }
            }
        }

        List<Coord> GetLine(Coord from, Coord to)
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
        List<List<Coord>> GetRegions(int tileType)
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
        List<Coord> GetRegionTiles(int startX, int startY)
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

        void SmoothMap()
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

        int GetSurroundingWallCount(int gridX, int gridY)
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

        bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
        }

        void RandomFillMap(int width, int height, int randomFillPercent)
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

        string StringifyCoordCollection(List<Coord> coordinates)
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
        public Map PopulateMap(Map emptyMap, int monsterStrength, int objectDencity)
        {
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
            CreateDwelling("CastleName", emptyMap, castlePosition, DwellingType.Castle);
            emptyMap.InitialHeroPosition = castlePosition;

            // 2. Place wood mines -> min 2 - max 4
            PlaceDwelling(emptyMap, "WoodMine", DwellingType.WoodMine, 2, 3);

            PlaceDwelling(emptyMap, "StoneMine", DwellingType.StoneMine, 2, 3);

            // TODO: don't forget the flood fill alg.
            // -> after getting the object position -> create a copy of PopulationRooms and update it with new object
            // after that -> run flood fill. 
            // If there are more than one (1) separate rooms.
            // discard the position and try again to find safe position.
            // repeat this process untill safe position is found.


            // 3. Place gold mine -> min 0 - max 2
            if (!MapIsFullForDwellings)
            {

            }

            // 4. Place other mines -> min 2 - max 4

            if (!MapIsFullForDwellings)
            {

            }

            // 5. Place monsters

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

                    Room randomRoom = this.GetRandomRoom(availibleRooms); //TODO: keep in mind that there might not be availible rooms and this can fail!
                    this.ResetTempVariables(randomRoom);

                    var monsterType = MonsterType.Spider;
                    var spaceRequired = SpaceRequirements.Monsters[monsterType];
                    Coord monsterPosition = this.TryGetSafePosition(randomRoom, PlacementStrategy.Random, 2, spaceRequired);
                    this.MarkPositionAsOccupied(monsterPosition, spaceRequired, randomRoom);
                    CreateMonster("Monster", emptyMap, monsterPosition, monsterType);
                }
            }

            emptyMap.Matrix = this.PopulatedMatrix; // the map is updated with all non-walkable cells
            return emptyMap;
        }

        private void CreateMonster(string name, Map emptyMap, Coord position, MonsterType type)
        {
            MonsterPack monster = new MonsterPack()
            {
                Type = MonsterType.Spider,
                X = position.X,
                Y = position.Y,
                Name = name + DateTime.Now.Ticks.ToString(),
                OccupiedTilesString = this.StringifyCoordCollection(this.ApplyPointShift(position, SpaceRequirements.Monsters[type])),
                Disposition = Disposition.Savage,
                // ItemReward = this.GetRandomLevel1Item(); - call cached version of database for ItemBluePrint with the required level
                Quantity = rand.Next(5, 10),
                RewardType = TreasureType.Gold,
            };

            emptyMap.MonsterPacks.Add(monster);
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

                Room randomRoom = this.GetRandomRoom(availibleRooms);
                this.ResetTempVariables(randomRoom);

                var spaceRequirement = SpaceRequirements.Dwellings[type];
                Coord dwellingPosition = this.TryGetSafePosition(randomRoom, PlacementStrategy.Random, 2, spaceRequirement);
                this.MarkPositionAsOccupied(dwellingPosition, spaceRequirement, randomRoom);
                CreateDwelling(name, emptyMap, dwellingPosition, type);
            }
        }

        private void CreateDwelling(string name, Map emptyMap, Coord position, DwellingType type)
        {
            Dwelling dwelling = new Dwelling()
            {
                Type = type,
                X = position.X,
                Y = position.Y,
                Name = name + DateTime.Now.Ticks.ToString(),
                //OwnerId = 1, //TODO: pass this as parameter in MapGeneration function.
                OccupiedTilesString = this.StringifyCoordCollection(this.ApplyPointShift(position, SpaceRequirements.Dwellings[type])),
            };

            emptyMap.Dwellings.Add(dwelling);
        }

        private void MarkPositionAsOccupied(Coord safePosition, List<Coord> additionalRequiredSpace, Room room)
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

        private Coord TryGetSafePosition(Room room, PlacementStrategy strategy, int edgeDistance, List<Coord> additionalRequiredSpace)
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

        private void ResetTempVariables(Room room)
        {
            this.TempRoomTiles = new List<Coord>(room.Tiles);
            this.TempEdgeRoomTiles = new List<Coord>(room.EdgeTiles);
        }

        private List<Coord> ApplyPointShift(Coord position, List<Coord> list)
        {
            List<Coord> updatedList = new List<Coord>();

            foreach (var item in list)
            {
                updatedList.Add(new Coord(position.X + item.X, position.Y + item.Y));
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
        private Coord GetRandomSafePosition(Room room, PlacementStrategy placementStrategy, int edgeDistance, List<Coord> additionalRequiredSpace)
        {
            Coord position = room.Tiles[rand.Next(room.Tiles.Count)];
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

        private Room GetRandomRoom(List<Room> rooms)
        {
            return rooms[rand.Next(rooms.Count)];
        }

        private class TempRoom : IComparable<TempRoom>
        {
            public List<Coord> tiles;
            public List<Coord> edgeTiles;
            public List<TempRoom> connectedRooms;
            public int roomSize;
            public bool isAccessibleFromMainRoom;
            public bool isMainRoom;

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

        private enum PlacementStrategy
        {
            Random = 0,
            NearEdge = 1,
            FarFromEdge = 2
        }
    }
}
