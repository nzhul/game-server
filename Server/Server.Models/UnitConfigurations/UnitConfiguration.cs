using Server.Models.MapEntities;
using System.Collections.Generic;

namespace Server.Models.UnitConfigurations
{
    public class UnitConfiguration
    {
        public int Id { get; set; }

        public CreatureType Type { get; set; }

        public Faction Faction { get; set; }

        public int MovementPointsBase { get; set; }

        public int ActionPointsBase { get; set; }

        public int MinDamageBase { get; set; }

        public int MinDamageIncrement { get; set; }

        public int MaxDamageBase { get; set; }

        public int MaxDamageIncrement { get; set; }

        public int HitpointsBase { get; set; }

        public int HitpointsIncrement { get; set; }

        public int ManaBase { get; set; }

        public int ManaIncrement { get; set; }

        public int ArmorBase { get; set; }

        public int ArmorIncrement { get; set; }

        public int EvasionBase { get; set; }

        public AttackType AttackType { get; set; }

        public ArmorType ArmorType { get; set; }

        public int BuildTime { get; set; }

        public int WoodCost { get; set; }

        public int OreCost { get; set; }

        public int GoldCost { get; set; }

        public int GemsCost { get; set; }

        public int FoodCost { get; set; }

        public ICollection<UnitConfigurationAbility> UnitConfigurationAbilitys { get; set; }

        public ICollection<UnitConfigurationUpgrade> UnitConfigurationUpgrades { get; set; }

        //public ICollection<Ability> Abilities { get; set; }

        //public ICollection<Upgrade> Upgrades { get; set; }
    }
}
