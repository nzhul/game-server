using App.Models.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Heroes
{
    public class Hero : IHero
    {
        private ICollection<Unit> units;

        public Hero()
        {
            this.units = new HashSet<Unit>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }


        public int SpellPower { get; set; }

        public int Intelligence { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual ICollection<Unit> Units
        {
            get { return this.units; }
            set { this.units = value; }
        }
    }
}
