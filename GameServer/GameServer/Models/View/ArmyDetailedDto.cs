using System.Collections.Generic;

namespace GameServer.Models.View
{
    public class ArmyDetailedDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? GameId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public Team Team { get; set; }

        public ICollection<UnitDetailedDto> Units { get; set; }
    }
}
