using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Units
{
    public abstract class Unit : IUnit
    {
        private static Random rand = new Random();


        public int Id { get; set; }

        public string Name { get; set; }

        public Enums.AttackType AttackTypes { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Damage
        {
            get
            {
                return rand.Next(this.MinDamage, this.MaxDamage);
            }
        }

        public int Health { get; set; }

        public int Speed { get; set; }

        public int Size { get; set; }

        public int Growth { get; set; }

        public string Description { get; set; }

        public int StackSize { get; set; }
    }
}
