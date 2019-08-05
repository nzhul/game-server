using System.Collections.Generic;

namespace Server.Models.Heroes.Units
{
    public class Upgrade
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public int WoodCost { get; set; }

        public int GoldCost { get; set; }

        public int TimeCost { get; set; }

        public ICollection<UnitConfigurationUpgrade> UnitConfigurationUpgrades { get; set; }

        //public int HitpointBonus { get; set; }

        //public int AttackDamageBonus { get; set; }

        //public bool UnlockAbility { get; set; } // i might not need this

        //public int AbilityLevelBonus { get; set; } // Ex: Upgrade "Poison Spears" ability with one level.

        //public string AbilityAffected { get; set; } // Ex: "Poison Spears"
    }
}