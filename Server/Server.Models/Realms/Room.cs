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

        private int _roomSize;

        public int RoomSize
        {
            get
            {
                return this._roomSize;
            }
            set
            {
                this._roomSize = value;
                this.FreeCellsLeft = value;
                this._minimumFreeCellsRequirementDwellings = (this.RoomSize * freePercentDwellings) / 100;
                this._minimumFreeCellsRequirementMonstersAndTreasure = (this.RoomSize * freePercentMonstersAndTreasure) / 100;
            }
        }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }

        [NotMapped]
        public int FreeCellsLeft { get; set; }

        [NotMapped]
        private readonly int freePercentDwellings = 95; // TODO: extract this in configuration

        [NotMapped]
        private readonly int freePercentMonstersAndTreasure = 91; // TODO: extract this in configuration

        [NotMapped]
        private int _minimumFreeCellsRequirementDwellings;

        [NotMapped]
        private int _minimumFreeCellsRequirementMonstersAndTreasure;

        [NotMapped]
        public bool AvailibleForDwellingPlacement
        {
            get
            {
                return this.FreeCellsLeft > this._minimumFreeCellsRequirementDwellings;
            }
        }

        [NotMapped]
        public bool AvailibleForMonsterOrTreasurePlacement
        {
            get
            {
                return this.FreeCellsLeft > this._minimumFreeCellsRequirementMonstersAndTreasure;
            }
        }
    }
}
