using App.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Towns.Human
{
    [Table("UnitProducers")]
    public class Barracks : Building, IUnitProducer
    {

        const HumanUnitType producedUnitType = HumanUnitType.Swordsman;
        const int productionRate = 14;
        const int level = 1;
        const string name = "Barracks";
        const string description = "Barracks is the place where the Swordmans train.";

        public Barracks()
        {

            this.ProducedUnitType = producedUnitType;
            this.ProductionRate = productionRate;
            this.Name = name;
            this.Description = description;
            this.ResourceCosts.Gold = 500;
            this.ResourceCosts.Wood = 10;
            this.ResourceCosts.Stone = 10;
        }

        public HumanUnitType ProducedUnitType { get; set; }

        public double ProductionRate { get; set; }
    }
}
