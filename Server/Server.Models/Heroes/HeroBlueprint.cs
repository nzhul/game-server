using Server.Models.Heroes.Units;

namespace Server.Models.Heroes
{
    public class HeroBlueprint : Entity
    {

        public string Description { get; set; }

        public string PortraitImgUrl { get; set; }

        public Faction Faction { get; set; }

        public HeroClass Class { get; set; }

        public int MovementPoints { get; set; }

        public int ActionPoints { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Hitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        public int Dodge { get; set; }

        public AttackType AttackType { get; set; }

        public ArmorType ArmorType { get; set; }
    }
}