using App.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Heroes
{
    public interface IHero
    {
        int Id { get; set; }

        string Name { get; set; }

        int Attack { get; set; }

        int Defense { get; set; }

        int SpellPower { get; set; }

        int Intelligence { get; set; }

        AppUser Owner { get; set; }

        ICollection<Unit> Units { get; set; }
    }
}
