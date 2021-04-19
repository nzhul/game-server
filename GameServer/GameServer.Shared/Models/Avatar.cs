using System.Collections.Generic;

namespace GameServer.Shared.Models
{
    public class Avatar
    {
        public int UserId { get; set; }

        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public Team Team { get; set; }
    }
}