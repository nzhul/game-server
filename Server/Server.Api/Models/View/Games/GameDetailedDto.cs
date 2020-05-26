using System.Collections.Generic;
using Server.Api.Controllers;
using Server.Api.Models.View.Avatars;
using Server.Api.Models.View.Realms;
using Server.Models.MapEntities;

namespace Server.Api.Models.View.Games
{
    public class GameDetailedDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MatrixString { get; set; }

        public ICollection<HeroDetailedDto> Heroes { get; set; }

        public ICollection<RoomDetailedDto> Rooms { get; set; }

        public ICollection<DwellingDetailedDto> Dwellings { get; set; }

        public ICollection<Treasure> Treasures { get; set; }

        public ICollection<AvatarDetailedDto> Avatars { get; set; }
    }
}
