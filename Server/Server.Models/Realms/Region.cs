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

        public string MatrixString { get; set; }

        [NotMapped]
        public int[,] Matrix
        {
            get
            {
                return this.ParseMatrix(this.MatrixString);
            }
        }

        private int[,] ParseMatrix(string matrixString)
        {
            throw new NotImplementedException();
        }

        public List<Room> Rooms { get; set; }

        #endregion
    }
}