using System.Collections.Generic;

namespace GameServer.Models.View
{
    public class GameDetailedDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MatrixString { get; set; }

        public ICollection<ArmyDetailedDto> Armies { get; set; }

        public ICollection<DwellingDetailedDto> Dwellings { get; set; }

        public ICollection<AvatarDetailedDto> Avatars { get; set; }
    }
}
