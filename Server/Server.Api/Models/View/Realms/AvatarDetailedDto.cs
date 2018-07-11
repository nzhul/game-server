using System.Collections.Generic;
using Server.Api.Models.View.Realms;

namespace Server.Api.Controllers
{
    public class AvatarDetailedDto
    {
        public int Id { get; set; }

        public int Wood { get; set; }

        public int Ore { get; set; }

        public int Gold { get; set; }

        public int Gems { get; set; }

        public ICollection<HeroDetailedDto> Heroes { get; set; }
    }
}