using System;
using System.Text;
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
                Map map = generator.GenerateMap(50, 50, 0, 1, 50, 50, 47);

                Console.Write(Program.StringifyMatrix(map.Matrix));

                //for (int x = 0; x < map.Matrix.GetLength(1); x++)
                //{
                //    for (int y = 0; y < map.Matrix.GetLength(0); y++)
                //    {
                //        Console.Write(map.Matrix[y, x]);
                //    }

                //    Console.WriteLine();
                //}
            }
        }

        static string StringifyMatrix(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    sb.Append(matrix[x, y].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
