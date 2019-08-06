﻿using Server.Models.MapEntities;

namespace Server.Api.Models.View.Realms
{
    public class UnitDetailedDto
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public CreatureType Type { get; set; }

        public int Quantity { get; set; }
    }
}