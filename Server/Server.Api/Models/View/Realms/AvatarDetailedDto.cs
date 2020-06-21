using System.Collections.Generic;
using Server.Models.Users;

namespace Server.Api.Controllers
{
    public class AvatarDetailedDto
    {
        public int Id { get; set; }

        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public int? UserId { get; set; }

        public Team Team { get; set; }

        public ICollection<int> Heroes { get; set; }

        public ICollection<int> Dwellings { get; set; }
    }
}