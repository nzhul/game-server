namespace Server.Models.UnitConfigurations
{
    public class UnitConfigurationAbility
    {
        public int UnitConfigurationId { get; set; }

        public UnitConfiguration UnitConfiguration { get; set; }

        public int AbilityId { get; set; }

        public Ability Ability { get; set; }
    }
}
