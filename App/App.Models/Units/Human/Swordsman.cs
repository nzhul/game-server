using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Units.Human
{
    [Table("Swordsmans")]
    public class Swordsman : Unit
    {
        const int minDamage = 1;
        const int maxDamage = 3;
        const string name = "Swordsman";
        const int attack = 4;
        const int defense = 5;
        const int health = 10;
        const int speed = 4;
        const int size = 1;
        const int growth = 14;

        public Swordsman(int attackAdd, int defenseAdd, int healthAdd, int speedAdd, int growthMult)
        {
            this.Name = name;
            this.Attack = attack + attackAdd;
            this.Defense = defense + defenseAdd;
            this.Health = health + healthAdd;
            this.Speed = speed + speedAdd;
            this.Size = size;
            this.Growth = growth * growthMult;
        }

        public int SwordsmanSPECIALDAMAGE { get; set; }

    }
}
