namespace Server.Models.Heroes.Units
{
    public class UnitConfigurationAbility
    {
        public int UnitConfigurationId { get; set; }

        public UnitConfiguration UnitConfiguration { get; set; }

        public int AbilityId { get; set; }

        public Ability Ability { get; set; }
    }
}
