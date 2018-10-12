using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Heroes;

namespace Server.Models.Realms
{
    public class Region : Entity
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int? RealmId { get; set; }

        public Realm Realm { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public Region()
        {
            this.Heroes = new Collection<Hero>();
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
                this._matrixString = value;
                this.Matrix = this.ParseMatrix(this._matrixString);
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

        public List<Room> Rooms { get; set; }

        #endregion
    }
}