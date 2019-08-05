using System.Collections.Generic;

namespace Server.Models.Heroes.Units
{
    public class Ability
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public bool IsHeroAbility { get; set; }

        /// <summary>
        /// How many times this ability can be upgraded. Ex: in Warcraft3 this is 3 for the heroes.
        /// </summary>
        public int Levels { get; set; }

        public int HealingAmount { get; set; }

        public int DamageAmount { get; set; }

        public ICollection<UnitConfigurationAbility> UnitConfigurationAbilitys { get; set; }
    }
}
