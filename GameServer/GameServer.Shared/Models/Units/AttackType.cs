namespace GameServer.Shared.Models.Units
{
    public enum AttackType
    {
        /// <summary>
        /// Most melee units
        /// Normal attacks do extra damage against Medium armor, and reduced damage to Fortified armor.
        /// </summary>
        Normal,

        /// <summary>
        /// Most ranged attackers
        /// Piercing attacks do extra damage to Unarmored units and Light armor, and reduced damage to Fortified, Medium armor, and Heroes.
        /// </summary>
        Piercing,

        /// <summary>
        /// Most siege units
        /// Siege attacks do extra damage to Fortified armor and Unarmored units, and reduced damage to Medium armor and Heroes.
        /// </summary>
        Siege,

        /// <summary>
        /// Most spellcasters
        /// Magic attacks do extra damage against Light and Heavy armor, and reduced damage to Medium, Fortified armor, and Heroes. 
        /// Magic attacks do 66% extra damage to ethereal units, and zero damage to magic-immune units.
        /// </summary>
        Magic,
        Hero
    }
}
