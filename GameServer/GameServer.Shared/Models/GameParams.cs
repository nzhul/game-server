using System.Collections.Generic;

namespace GameServer.Shared.Models
{
    public class GameParams
    {
        /// <summary>
        /// Defines the map template to be used in the game.
        /// </summary>
        public MapTemplate MapTemplate { get; set; }

        /// <summary>
        /// Bigger size means all zones will scale up proportionaly.
        /// </summary>
        public int MapSize { get; set; }

        /// <summary>
        /// Higher dificulty means less resources at start and tougher enemies.
        /// </summary>
        public int MapDifficulty { get; set; }

        public IList<Player> Players { get; set; }
    }

    public class Player
    {
        public int UserId { get; set; }

        public CreatureType StartingClass { get; set; }

        public Team Team { get; set; }
    }

    public enum MapTemplate
    {
        Default,
        Cross,
        Small
    }
}
