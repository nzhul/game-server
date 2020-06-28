using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Armies;
using Server.Models.MapEntities;
using Server.Models.Users;

namespace Server.Models.Realms
{
    public class Game : Entity
    {
        public string Name { get; set; }

        public ICollection<Army> Armies { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public ICollection<Treasure> Treasures { get; set; }

        public ICollection<Dwelling> Dwellings { get; set; }

        public ICollection<User> Users { get; set; }

        public Game()
        {
            this.Armies = new Collection<Army>();
            this.Users = new Collection<User>();
            this.Rooms = new Collection<Room>();
            this.Treasures = new Collection<Treasure>();
            this.Dwellings = new Collection<Dwelling>();
        }


        #region MapData

        private string _matrixString;

        public string MatrixString
        {
            get
            {
                return this._matrixString;
            }
            set
            {
                if (value != null)
                {
                    this._matrixString = value;
                    this.Matrix = this.ParseMatrix(this._matrixString);
                }
            }
        }

        [NotMapped]
        public int[,] Matrix { get; private set; }

        private int[,] ParseMatrix(string matrixString)
        {
            string[] lines = matrixString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            int height = lines.Length;
            int width = lines[0].Length;

            int[,] parsedMatrix = new int[height, width];
            for (int y = 0; y < height; y++)
            {
                string line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    parsedMatrix[y, x] = (int)char.GetNumericValue(line[x]);
                }
            }

            return parsedMatrix;
        }

        #endregion
    }
}