using System;
using System.Collections.Generic;
using System.Diagnostics;
using Server.Data.Generators;
using Server.Models;
using Server.Models.Realms;

namespace Server.GeneratorTesting
{
    /// <summary>
    /// characters:
    /// '\u25A0' -> ■
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            bool debugMode = false;

            Random r = new Random();
            ConsoleKey key = Console.ReadKey().Key;
            while (key == ConsoleKey.R || key == ConsoleKey.D)
            {
                if (key == ConsoleKey.D)
                {
                    debugMode = true;
                }
                else if (key == ConsoleKey.R)
                {
                    debugMode = false;
                }

                Console.Clear();

                IMapGenerator generator = new MapGenerator();
                Map map = null;

                bool generationIsFailing = true;
                bool retryLimitNotReached = true;
                int retryLimit = 10;
                int currentRetries = 0;

                while (generationIsFailing && retryLimitNotReached)
                {
                    try
                    {
                        stopwatch.Start();
                        map = generator.GenerateMap(36, 36, 0, 1, 0, 0, 30);
                        //map = generator.GenerateMap(100, 76, 0, 1, 100, 10, 45);
                        map = generator.PopulateMap(map, 50, 50);
                        stopwatch.Stop();
                        generationIsFailing = false;
                    }
                    catch (Exception ex)
                    {
                        if (currentRetries <= retryLimit)
                        {
                            currentRetries++;
                            Console.WriteLine("Current retries: " + currentRetries);
                        }
                        else
                        {
                            retryLimitNotReached = false;
                            throw ex;
                        }
                    }
                }

                if (debugMode)
                {
                    Console.Write("  ");
                    for (int x = 0; x < map.Matrix.GetLength(0); x++)
                    {
                        Console.Write(x.ToString().PadLeft(2));
                    }
                    Console.WriteLine();
                }

                for (int x = 0; x < map.Matrix.GetLength(1); x++)
                {
                    if (debugMode)
                    {
                        Console.Write(x.ToString().PadLeft(2));
                    }

                    for (int y = 0; y < map.Matrix.GetLength(0); y++)
                    {
                        if (map.Matrix[y, x] == 1)
                        {
                            if (debugMode)
                            {
                                Console.Write(" \u2588");
                            }
                            else
                            {
                                Console.Write("\u2588");
                            }
                        }
                        else if (map.Matrix[y, x] == 2)
                        {
                            if (debugMode)
                            {
                                Console.Write(" X");
                            }
                            else
                            {
                                Console.Write("X");
                            }

                        }
                        else if (map.Matrix[y, x] == 3)
                        {
                            if (debugMode)
                            {
                                Console.Write(" O");
                            }
                            else
                            {
                                Console.Write("O");
                            }
                            
                        }
                        else
                        {
                            if (debugMode)
                            {
                                Console.Write(" \u2591");
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                            
                        }
                    }

                    if (debugMode)
                    {
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                //PaintRooms(map.Rooms);
                //PaintEdges(map.Rooms);
                //PaintHero(map, r);

                if (debugMode)
                {
                    Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(1) * 2);
                }
                else
                {
                    Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(1));
                }

                Console.WriteLine();
                Console.WriteLine(stopwatch.Elapsed);

                key = Console.ReadKey().Key;
            }
        }

        private static void PaintEdges(List<Room> rooms)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (Room room in rooms)
            {
                foreach (Coord coord in room.EdgeTiles)
                {
                    Console.SetCursorPosition(coord.X, coord.Y);
                    Console.Write('\u25A0');
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PaintHero(Map map, Random r)
        {
            Coord randomPositionInMainRoom = map.Rooms[0].Tiles[r.Next(map.Rooms[0].Tiles.Count)];
            Console.SetCursorPosition(randomPositionInMainRoom.X, randomPositionInMainRoom.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('\u2588');
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(0));
        }

        private static void PaintRooms(List<Room> rooms)
        {
            int colorIndex = 9;
            foreach (Room room in rooms)
            {
                if (colorIndex <= 14)
                {
                    Console.ForegroundColor = (ConsoleColor)colorIndex;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                foreach (Coord coord in room.Tiles)
                {
                    Console.SetCursorPosition(coord.X, coord.Y);
                    //Console.Write('\u2591');
                    Console.Write('\u2588');
                }
                Console.ForegroundColor = ConsoleColor.White;
                colorIndex++;
            }
        }

        private static bool IsInRoom(int x, int y, Room room)
        {
            bool isInRoom = false;

            foreach (var item in room.Tiles)
            {
                if (item.X == x && item.Y == y)
                {
                    isInRoom = true;
                }
            }

            return isInRoom;
        }
    }
}
