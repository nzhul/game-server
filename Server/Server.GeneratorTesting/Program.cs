using System;
using System.Collections.Generic;
using System.Diagnostics;
using Server.Data.Generators;
using Server.Models;
using Server.Models.Realms;
using Server.Models.Realms.Input;

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

                var config = new GameParams()
                {
                    MapTemplate = MapTemplate.Default
                };

                //var sw = new Stopwatch();
                //sw.Start();
                map = generator.TryGenerateMap(config);
                //sw.Stop();
                //Console.WriteLine(sw.ElapsedMilliseconds);
                //Console.WriteLine();


                if (debugMode)
                {
                    Console.Write("  ");
                    for (int x = 0; x < map.Matrix.GetLength(0); x++)
                    {
                        Console.Write(x.ToString().PadLeft(2));
                    }
                    Console.WriteLine();
                }

                for (int row = 0; row < map.Matrix.GetLength(0); row++)
                {
                    if (debugMode)
                    {
                        Console.Write(row.ToString().PadLeft(2));
                    }

                    for (int col = 0; col < map.Matrix.GetLength(1); col++)
                    {
                        if (map.Matrix[row, col] == 1)
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
                        else if (map.Matrix[row, col] == 2)
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
                        else if (map.Matrix[row, col] == 3)
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
                PaintPassages(map.Matrix);

                //if (debugMode)
                //{
                //    Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(1) * 2);
                //}
                //else
                //{
                //    Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(1));
                //}

                //Console.WriteLine();
                //Console.WriteLine(stopwatch.Elapsed);

                key = Console.ReadKey().Key;
            }
        }

        private static void PaintPassages(int[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] == 99)
                    {
                        Console.SetCursorPosition(col, row);
                        Console.Write("$");
                    }
                }
            }
        }

        private static void PaintEdges(List<TempRoom> rooms)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var room in rooms)
            {
                foreach (Coord coord in room.edgeTiles)
                {
                    Console.SetCursorPosition(coord.Y, coord.X);
                    Console.Write('\u25A0');
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PaintHero(Map map, Random r)
        {
            Coord randomPositionInMainRoom = map.Rooms[0].tiles[r.Next(map.Rooms[0].tiles.Count)];
            Console.SetCursorPosition(randomPositionInMainRoom.X, randomPositionInMainRoom.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('\u2588');
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(map.Matrix.GetLength(1), map.Matrix.GetLength(0));
        }

        private static void PaintRooms(List<TempRoom> rooms)
        {
            int colorIndex = 1;
            foreach (TempRoom room in rooms)
            {
                if (colorIndex <= 14)
                {
                    Console.ForegroundColor = (ConsoleColor)colorIndex;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                foreach (Coord coord in room.tiles)
                {
                    Console.SetCursorPosition(coord.Y, coord.X);
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
