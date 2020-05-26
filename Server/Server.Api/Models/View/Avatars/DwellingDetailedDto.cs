using Server.Models.MapEntities;
using Server.Models.Users;

namespace Server.Api.Models.View.Avatars
{
    public class DwellingDetailedDto
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Team Team { get; set; }

        public DwellingType Type { get; set; }

        public int? OwnerId { get; set; }

        public int GameId { get; set; }

        public int? GuardianId { get; set; }
    }
}