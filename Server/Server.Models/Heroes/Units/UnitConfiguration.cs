using Server.Models.MapEntities;

namespace Server.Models.Heroes.Units
{
    public class UnitConfiguration
    {
        public CreatureType Type { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Hitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        public int Speed { get; set; }

        public int CreatureLevel { get; set; }

        public int BuildTime { get; set; }

        public int WoodCost { get; set; }

        public int OreCost { get; set; }

        public int GoldCost { get; set; }

        public int GemsCost { get; set; }

        public int FoodCost { get; set; }

        //public IList<Ability> Abilities { get; set; }
    }
}
