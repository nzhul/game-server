using Server.Models.MapEntities;

namespace Server.Models.Heroes.Units
{
    public class Unit : Entity
    {
        public int StartX { get; set; }

        public int StartY { get; set; }

        public CreatureType Type { get; set; }

        public int Quantity { get; set; }

        public int? HeroId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}