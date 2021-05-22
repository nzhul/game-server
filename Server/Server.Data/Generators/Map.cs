using System.Collections.Generic;
using System.Text;
using Server.Models;
using Server.Models.Armies;
using Server.Models.MapEntities;

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

        public void Stringify()
        {
            this.MatrixString = this.StringifyMatrix(this.Matrix);
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

        public Coord Position { get; set; }

        public string MatrixString { get; private set; }

        public List<TempRoom> Rooms { get; set; }

        public List<Army> Armies { get; set; }

        public List<Treasure> Treasures { get; set; }

        public List<Dwelling> Dwellings { get; set; }

        public string Seed { get; set; }

        public Map()
        {
            this.Dwellings = new List<Dwelling>();
            this.Rooms = new List<TempRoom>();
            this.Armies = new List<Army>();
            this.Treasures = new List<Treasure>();
        }
    }
}
