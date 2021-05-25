using Server.Models.MapEntities;
using Server.Models.UnitConfigurations;

namespace Server.Api.Models.View.UnitConfigurations
{
    public class UnitConfigurationView
    {
        public int Id { get; set; }

        public CreatureType Type { get; set; }

        public Faction Faction { get; set; }

        public int MovementPoints { get; set; }

        public int ActionPoints { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Hitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        public AttackType AttackType { get; set; }

        public ArmorType ArmorType { get; set; }

        public int CreatureLevel { get; set; }

        public int BuildTime { get; set; }

        public int WoodCost { get; set; }

        public int OreCost { get; set; }

        public int GoldCost { get; set; }

        public int GemsCost { get; set; }

        public int FoodCost { get; set; }
    }
}