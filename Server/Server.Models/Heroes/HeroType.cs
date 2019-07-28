namespace Server.Models.Heroes
{
    public enum HeroType
    {
        /// <summary>
        /// Normal alive hero with army or alone
        /// </summary>
        Normal,
        /// <summary>
        /// The hero is dead but the army can move if there are some units
        /// </summary>
        Dead,
        /// <summary>
        /// Hero is only a placeholder that acts as a group for the units.
        /// </summary>
        Placeholder
    }
}
