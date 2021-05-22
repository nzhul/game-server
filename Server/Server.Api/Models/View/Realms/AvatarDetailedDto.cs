using Server.Models.Users;

namespace Server.Api.Controllers
{
    public class AvatarDetailedDto
    {
        public int UserId { get; set; }

        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public Team Team { get; set; }

        public string VisitedString { get; set; }
    }
}