using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Parsers;
using Server.Models.Users;

namespace Server.Models.MapEntities
{
    public class Dwelling : MapEntity
    {
        public DwellingType Type { get; set; }

        public int OwnerId { get; set; }

        public Avatar Owner { get; set; }

        private string _occupiedTilesString;

        public string OccupiedTilesString
        {
            get
            {
                return this._occupiedTilesString;
            }
            set
            {
                if (value != null)
                {
                    this._occupiedTilesString = value;
                    this.OccupiedTiles = CommonParser.ParseTiles(this._occupiedTilesString);
                }
            }
        }

        [NotMapped]
        public List<Coord> OccupiedTiles { get; set; }
    }
}
