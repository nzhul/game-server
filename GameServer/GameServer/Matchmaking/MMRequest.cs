using System;
using GameServer.Utilities;
using NetworkingShared.Enums;

namespace GameServer.Matchmaking
{
    public class MMRequest
    {
        public ServerConnection Connection { get; set; }

        public DateTime SearchStart { get; set; }

        public CreatureType StartingClass { get; set; }

        public bool MatchFound { get; set; }

        public int SearchRadius
        {
            get
            {
                return (int)TimeInQueue.TotalSeconds * 10; // 60 seconds * 10 = 600
            }
        }

        public Range<int> SearchRange
        {
            get
            {
                return new Range<int>(this.Connection.User.Mmr - this.SearchRadius, this.Connection.User.Mmr + this.SearchRadius);
            }
        }

        public TimeSpan TimeInQueue
        {
            get
            {
                return DateTime.UtcNow - this.SearchStart;
            }
        }
    }
}
