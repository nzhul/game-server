using App.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Towns.Common
{
    [Table("ResourceProducers")]
    public class VillageHall : Building, IResourceProducer
    {
        const ResourceType producedResourceType = ResourceType.Gold;
        const int productionRate = 500;
        const int level = 1;
        const string name = "Village Hall";
        const string description = "The Village hall is the main building in your Castle. Upgrade it to get more gold per hour!";

        public VillageHall()
        {

            this.ProducedResourceType = producedResourceType;
            this.ProductionRate = productionRate;
            this.Level = level;
            this.Name = name;
            this.Description = description;
            this.ResourceCosts.Gold = 500;
        }


        public ResourceType ProducedResourceType { get; set; }

        public int ProductionRate { get; set; }
    }
}
