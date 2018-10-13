using System.Collections.Generic;
using Server.Models;

namespace Server.Api.Models.View.Realms
{
    public class RoomDetailedDto
    {
        public int Id { get; set; }

        public string TilesString { get; set; }

        public string EdgeTilesString { get; set; }

        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }

        public ICollection<HeroDetailedDto> Heroes { get; set; }
    }
}