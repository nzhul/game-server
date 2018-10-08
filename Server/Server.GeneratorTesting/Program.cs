using System;
using Server.Data.Generators;

namespace Server.GeneratorTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            while (Console.ReadKey().Key == ConsoleKey.R)
            {
                Console.Clear();

                IMapGenerator generator = new MapGenerator();
                Map map = generator.GenerateMap(50, 20, 0, 1, 50, 50, 47);

                for (int x = 0; x < map.Matrix.GetLength(1); x++)
                {
                    for (int y = 0; y < map.Matrix.GetLength(0); y++)
                    {
                        Console.Write(map.Matrix[y, x]);
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
