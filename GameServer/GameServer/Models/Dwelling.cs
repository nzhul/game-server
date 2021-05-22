using System;
using System.Collections.Generic;
using GameServer.MapGeneration;
using NetworkShared.Models;

namespace GameServer.Models
{
    public class Dwelling : MapEntity
    {
        public DwellingType Type { get; set; }

        public int? UserId { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public int? GuardianId { get; set; }

        public Army Guardian { get; set; }

        public int EndX { get; set; }

        public int EndY { get; set; }

        private string _occupiedTilesString;

        // TODO: Delete string properties
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

        public List<Coord> OccupiedTiles { get; set; }

        public Guid? Link { get; set; }

        private string _visitorsString;

        // TODO: Delete string properties
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
