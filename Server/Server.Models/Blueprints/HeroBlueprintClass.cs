using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Models.Blueprints
{
    public class HeroBlueprintClass
    {
        #region Meta
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region Probabilities

        public int AttackGainChance { get; set; }

        public int DefenseGainChance { get; set; }

        public int MagicGainChance { get; set; }

        public int MagicPowerGainChance { get; set; }
        
        #endregion
    }
}
