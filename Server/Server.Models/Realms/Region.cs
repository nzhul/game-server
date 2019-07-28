using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Castles;
using Server.Models.Heroes;
using Server.Models.MapEntities;

namespace Server.Models.Realms
{
    public class Region : Entity
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int? RealmId { get; set; }

        public Realm Realm { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public ICollection<Castle> Castles { get; set; }

        public ICollection<Treasure> Treasures { get; set; }

        public ICollection<Dwelling> Dwellings { get; set; }

        public Region()
        {
            this.Rooms = new Collection<Room>();
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
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
            int[,] parsedMatrix = new int[lines[0].Length, lines.Length];
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    parsedMatrix[col, row] = (int)char.GetNumericValue(line[col]);
                }
            }

            return parsedMatrix;
        }

        [NotMapped]
        public Coord InitialHeroPosition { get; set; }

        #endregion
    }
}