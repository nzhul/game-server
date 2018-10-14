using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Parsers;

namespace Server.Models.Realms
{
    public class Room : Entity
    {
        private string _tilesString;

        /// <summary>
        /// CSV of all tile coordinates:
        /// Ex: 0:0,0:1,0:2, where left is X and right is Y
        /// </summary>
        public string TilesString
        {
            get
            {
                return this._tilesString;
            }
            set
            {
                if (value != null)
                {
                    this._tilesString = value;
                    this.Tiles = CommonParser.ParseTiles(this._tilesString);
                }
            }
        }

        [NotMapped]
        public List<Coord> Tiles { get; set; }


        private string _edgeTilesString;
        /// <summary>
        /// CSV of all tile coordinates:
        /// Ex: 0:0,0:1,0:2, where left is X and right is Y
        /// </summary>
        public string EdgeTilesString
        {
            get
            {
                return this._edgeTilesString;
            }
            set
            {
                if (value != null)
                {
                    this._edgeTilesString = value;
                    this.EdgeTiles = CommonParser.ParseTiles(this._edgeTilesString);
                }
            }
        }

        [NotMapped]
        public List<Coord> EdgeTiles { get; set; }

        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }
    }
}
