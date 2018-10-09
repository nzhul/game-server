using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Castles;
using Server.Models.Heroes;

namespace Server.Models.Realms
{
    public class Room : Entity
    {
        public ICollection<Hero> Heroes { get; set; }

        public ICollection<Castle> Castles { get; set; }


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
                this._tilesString = value;
                this.Tiles = this.ParseTiles(this._tilesString);
            }
        }

        private List<Coord> ParseTiles(string value)
        {
            List<Coord> roomCoordinates = new List<Coord>();

            string[] tilesParts = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tilesParts.Length; i++)
            {
                string[] coords = tilesParts[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                roomCoordinates.Add(new Coord() { X = int.Parse(coords[0]), Y = int.Parse(coords[1]) });
            }

            return roomCoordinates;
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
                this._edgeTilesString = value;
                this.EdgeTiles = this.ParseTiles(this._edgeTilesString);
            }
        }

        public List<Coord> EdgeTiles { get; set; }

        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }

        // public ICollection<MonsterCamp> MonsterCamps { get; set; }

        // public ICollection<Interactable> Interactables { get; set; }

        // public ICollection<Mine> Mines { get; set; }

        public Room()
        {
            this.Heroes = new Collection<Hero>();
            this.Castles = new Collection<Castle>();
        }
    }
}
