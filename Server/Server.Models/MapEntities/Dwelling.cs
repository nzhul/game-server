using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Armies;
using Server.Models.Parsers;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Models.MapEntities
{
    public class Dwelling : MapEntity
    {
        public Dwelling()
        {
            //this.AvatarDwellings = new Collection<AvatarDwelling>();
        }

        //public virtual ICollection<AvatarDwelling> AvatarDwellings { get; set; }

        public DwellingType Type { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public int? GuardianId { get; set; }

        public Army Guardian { get; set; }

        public int EndX { get; set; }

        public int EndY { get; set; }

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

        [NotMapped]
        public Guid? Link { get; set; }

        private string _visitorsString;

        public string VisitorsString
        {
            get
            {
                return this._visitorsString;
            }
            set
            {
                if (value != null)
                {
                    this._visitorsString = value;
                    this.Visitors = CommonParser.ParseCsvIds(this._visitorsString);
                }
            }
        }

        /// <summary>
        /// Adding visitor directly to this list won't be recorded in the database.
        /// Please use AddVisitor method.
        /// </summary>
        [NotMapped]
        public List<int> Visitors { get; private set; }

        public void AddVisitor(int visitorId)
        {
            if (!this.Visitors.Contains(visitorId))
            {
                this.Visitors.Add(visitorId);
                this._visitorsString = string.Join(',', this.Visitors);
            }
        }
    }
}
