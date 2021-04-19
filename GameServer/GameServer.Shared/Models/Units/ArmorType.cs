namespace GameServer.Shared.Models.Units
{
    public enum ArmorType
    {
        /// <summary>
        /// Most spellcasters
        /// Unarmored takes extra damage from Piercing, and Siege attacks. 
        /// </summary>
        Unarmored,

        /// <summary>
        /// Most flying units
        /// Light armor takes extra damage from Piercing and Magic attacks.
        /// </summary>
        Light,

        /// <summary>
        /// Most ranged attackers
        /// Medium armor takes extra damage from Normal attacks, reduces damage from Piercing, Magic, and Siege attacks.
        /// </summary>
        Medium,

        /// <summary>
        /// Most melee units
        /// Heavy armor takes extra damage from Magic attacks.
        /// </summary>
        Heavy,

        /// <summary>
        /// Heroes take reduced damage from Piercing, Magic, Spell, and Siege attacks.
        /// </summary>
        Hero,

        /// <summary>
        /// Special siege units or In-Battle buildings (Ex: Towers).
        /// Fortified armor greatly reduces Piercing, Magic, Hero, and Normal attacks, but takes extra damage from Siege attacks.
        /// </summary>
        Fortified
    }
}
