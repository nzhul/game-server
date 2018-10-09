using System;
using System.Text;
using Server.Data.Generators;
using Server.Models;
using Server.Models.Realms;

namespace Server.GeneratorTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            while (Console.ReadKey().Key == ConsoleKey.R)
            {
                Console.Clear();

                IMapGenerator generator = new MapGenerator();
                Map map = generator.GenerateMap(50, 100, 0, 1, 10, 10, 50);

                Coord randomPositionInMainRoom = map.Rooms[0].Tiles[r.Next(map.Rooms[0].Tiles.Count)];
                map.Matrix[randomPositionInMainRoom.X, randomPositionInMainRoom.Y] = 2;

                for (int x = 0; x < map.Matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < map.Matrix.GetLength(1); y++)
                    {
                        if (IsInRoom(x, y, map.Rooms[0]))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }

                        if (map.Matrix[x, y] == 1)
                        {
                            Console.Write('\u2588');
                        }
                        else if (map.Matrix[x, y] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('\u2588');
                        }
                        else
                        {
                            Console.Write('\u2591');
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine();
                }
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
