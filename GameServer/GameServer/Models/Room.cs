using System.Collections.Generic;
using GameServer.MapGeneration;
using NetworkShared.Models;

namespace GameServer.Models
{
    public class Room : Entity
    {
        private string _tilesString;

        // TODO: Delete string properties
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

        public List<Coord> EdgeTiles { get; set; }

        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }
    }
}
