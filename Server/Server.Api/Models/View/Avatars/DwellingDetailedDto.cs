using Server.Models.MapEntities;

namespace Server.Api.Models.View.Avatars
{
    public class DwellingDetailedDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public DwellingType Type { get; set; }

        public int? OwnerId { get; set; }

        public int RegionId { get; set; }
    }
}