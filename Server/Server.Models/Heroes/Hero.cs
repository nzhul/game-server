using Server.Models.Blueprints;

namespace Server.Models.Heroes
{
    public class Hero
    {
        public int Id { get; set; }

        public int Attack { get; set; }

        public int Defence { get; set; }

        public int Magic { get; set; }

        public int MagicPower { get; set; }

        public int PersonalAttack { get; set; }

        public int PersonalDefense { get; set; }

        public int Dodge { get; set; }

        public int Health { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int MagicResistance { get; set; }

        public int BlueprintId { get; set; }

        public HeroBlueprint Blueprint { get; set; }
    }
}
