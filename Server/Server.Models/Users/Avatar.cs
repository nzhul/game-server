using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Server.Models.Parsers;

namespace Server.Models.Users
{

    [Owned]
    public class Avatar
    {
        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public Team Team { get; set; }

        private string _visitedString;

        public string VisitedString
        {
            get
            {
                return this._visitedString;
            }
            set
            {
                if (value != null)
                {
                    this._visitedString = value;
                    this.Visited = CommonParser.ParseCsvIds(this._visitedString);
                }
            }
        }

        /// <summary>
        /// Adding visitor directly to this list won't be recorded in the database.
        /// Please use AddVisitor method.
        /// </summary>
        [NotMapped]
        public List<int> Visited { get; private set; }

        public void AddVisited(int visitorId)
        {
            if (!this.Visited.Contains(visitorId))
            {
                this.Visited.Add(visitorId);
                this._visitedString = string.Join(',', this.Visited);
            }
        }
    }

    public enum Team
    {
        Neutral,
        Team1,
        Team2,
        Team3,
        Team4,
        Team5,
        Team6,
        Team7,
        Team8
    }
}
