using System.Collections.Generic;

namespace Server.Api.Models.View.Realms
{
    public class RegionDetailedDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int? RealmId { get; set; }

        public string MatrixString { get; set; }

        public ICollection<RoomDetailedDto> Rooms { get; set; }

        public ICollection<HeroDetailedDto> Heroes { get; set; }
    }
}
