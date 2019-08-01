using Server.Models.MapEntities;

namespace Server.Models.Heroes.Units
{
    public class Unit : Entity
    {
        public int X { get; set; }

        public int Y { get; set; }

        public CreatureType Type { get; set; }

        public int Quantity { get; set; }

        public int? HeroId { get; set; }

        public virtual Hero Hero { get; set; }
    }
}