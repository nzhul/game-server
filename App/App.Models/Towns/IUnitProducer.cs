using App.Models.Enums;
using App.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Towns
{
    public interface IUnitProducer
    {
        HumanUnitType ProducedUnitType { get; set; }

        double ProductionRate { get; set; }
    }
}
