﻿namespace Server.Models.Heroes.Units
{
    public class UnitConfigurationUpgrade
    {
        public int UnitConfigurationId { get; set; }

        public UnitConfiguration UnitConfiguration { get; set; }

        public int UpgradeId { get; set; }

        public Upgrade Upgrade { get; set; }
    }
}
