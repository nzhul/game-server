using System.Collections.Generic;
using System.Text;
using Server.Models.Heroes;
using Server.Models.MapEntities;
using Server.Models.Realms;

namespace Server.Data.Generators
{
    public class Map
    {
        private int[,] _matrix;

        public int[,] Matrix
        {
            get
            {
                return this._matrix;
            }
            set
            {
                this._matrix = value;
                this.MatrixString = this.StringifyMatrix(this._matrix);
            }
        }

        private string StringifyMatrix(int[,] matrix)
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

        public string MatrixString { get; private set; }

        public List<Room> Rooms { get; set; }

        public List<Hero> Heroes { get; set; }

        public List<MonsterPack> MonsterPacks { get; set; }

        public List<Treasure> Treasures { get; set; }

        public List<Dwelling> Dwellings { get; set; }

        public string Seed { get; set; }
    }
}
