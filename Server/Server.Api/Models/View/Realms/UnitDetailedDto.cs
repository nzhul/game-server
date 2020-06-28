﻿using Server.Models.MapEntities;

namespace Server.Api.Models.View.Realms
{
    public class UnitDetailedDto
    {
        public int Id { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Level { get; set; }

        public int? GameId { get; set; }

        public int? AvatarId { get; set; }

        public int? ArmyId { get; set; }

        public CreatureType Type { get; set; }

        public int Quantity { get; set; }
    }
}
