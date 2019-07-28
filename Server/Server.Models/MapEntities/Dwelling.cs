using Server.Models.Parsers;
using Server.Models.Realms;
using Server.Models.Users;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.MapEntities
{
    public class Dwelling : MapEntity
    {
        public Dwelling()
        {
            this.AvatarDwellings = new Collection<AvatarDwelling>();
        }

        public virtual ICollection<AvatarDwelling> AvatarDwellings { get; set; }

        public DwellingType Type { get; set; }

        public int? OwnerId { get; set; }

        public Avatar Owner { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }

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
