using System;
using System.Collections.Generic;
using System.Text;
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
            Random r = new Random();
            while (Console.ReadKey().Key == ConsoleKey.R)
            {
                Console.Clear();

                IMapGenerator generator = new MapGenerator();
                Map map = generator.GenerateMap(60, 130, 0, 1, 50, 50, 49);

                for (int x = 0; x < map.Matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < map.Matrix.GetLength(1); y++)
                    {
                        if (map.Matrix[x, y] == 1)
                        {
                            Console.Write('\u2588');
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }

                    Console.WriteLine();
                }

                PaintRooms(map.Rooms);
                PaintEdges(map.Rooms);
                PaintHero(map, r);
            }
        }

        private static void PaintEdges(List<Room> rooms)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (Room room in rooms)
            {
                foreach (Coord coord in room.EdgeTiles)
                {
                    Console.SetCursorPosition(coord.Y, coord.X);
                    Console.Write('\u25A0');
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PaintHero(Map map, Random r)
        {
            Coord randomPositionInMainRoom = map.Rooms[0].Tiles[r.Next(map.Rooms[0].Tiles.Count)];
            Console.SetCursorPosition(randomPositionInMainRoom.Y, randomPositionInMainRoom.X);
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

        static string StringifyMatrix(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == 1)
                    {
                        sb.Append('\u2588');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
