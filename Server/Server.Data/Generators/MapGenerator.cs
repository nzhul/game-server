using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;

namespace Server.Data.Generators
{
    public class MapGenerator : IMapGenerator
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public int[,] Matrix { get; private set; }

        public int PassageRadius { get; private set; }

        public Map GenerateMap(int width = 128,
            int height = 76,
            int borderSize = 0,
            int passageRadius = 1,
            int minRoomSize = 50,
            int minWallSize = 50,
            int randomFillPercent = 47,
            string seed = "")
        {
            this.Width = width;
            this.Height = height;
            this.Matrix = new int[width, height];
            this.PassageRadius = passageRadius;

            Map map = new Map();
            if (string.IsNullOrEmpty(seed))
            {
                seed = DateTime.UtcNow.Ticks.ToString();
            }
            List<Room> rooms = new List<Room>();

            this.RandomFillMap(seed, width, height, randomFillPercent);

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
            map.Seed = seed;
            return map;
        }

        private List<Models.Realms.Room> TransformRoomEntities(List<Room> rooms)
        {
            List<Models.Realms.Room> dbRooms = new List<Models.Realms.Room>();

            foreach (var room in rooms)
            {
                Models.Realms.Room newRoom = new Models.Realms.Room();
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
        List<Room> ProcessMap(int minWallSize, int minRoomSize)
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

            List<Room> survivingRooms = new List<Room>();

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
                    survivingRooms.Add(new Room(roomRegion, this.Matrix));
                }
            }

            survivingRooms.Sort();

            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;

            ConnectClosestRooms(survivingRooms);

            return survivingRooms;
        }

        void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
        {
            List<Room> roomListA = new List<Room>();
            List<Room> roomListB = new List<Room>();

            if (forceAccessibilityFromMainRoom)
            {
                foreach (Room room in allRooms)
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
            Room bestRoomA = new Room();
            Room bestRoomB = new Room();
            bool possibleConnectionFound = false;

            foreach (Room roomA in roomListA)
            {
                if (!forceAccessibilityFromMainRoom)
                {
                    possibleConnectionFound = false;
                    if (roomA.connectedRooms.Count > 0)
                    {
                        continue;
                    }
                }

                foreach (Room roomB in roomListB)
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
        void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
        {
            Room.ConnectRooms(roomA, roomB);
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

        void RandomFillMap(string seed, int width, int height, int randomFillPercent)
        {
            Random r = new Random(seed.GetHashCode());

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

        private class Room : IComparable<Room>
        {
            public List<Coord> tiles;
            public List<Coord> edgeTiles;
            public List<Room> connectedRooms;
            public int roomSize;
            public bool isAccessibleFromMainRoom;
            public bool isMainRoom;

            public Room()
            {
            }

            public Room(List<Coord> tiles, int[,] map)
            {
                this.tiles = tiles;
                this.roomSize = tiles.Count;
                connectedRooms = new List<Room>();
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
                    foreach (Room connectedRoom in connectedRooms)
                    {
                        connectedRoom.SetAccessibleFromMainRoom();
                    }
                }
            }

            public static void ConnectRooms(Room roomA, Room roomB)
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

            public int CompareTo(Room other)
            {
                return other.roomSize.CompareTo(this.roomSize);
            }

            public bool IsConnected(Room otherRoom)
            {
                return connectedRooms.Contains(otherRoom);
            }
        }
    }
}
