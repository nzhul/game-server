using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models.Enums;

namespace App.Models.Units
{
    public interface IUnit
    {
        int Id { get; set; }

        string Name { get; set; }

        AttackType AttackTypes { get; set;}

        int Attack { get; set; }

        int Defense { get; set; }

        int MinDamage { get; set; }

        int MaxDamage { get; set; }

        int Health { get; set; }

        int Speed { get; set; }

        int Size { get; set; }

        int Growth { get; set; }

        string Description { get; set; }

        int StackSize { get; set; }
    }
}
