using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Units
{
    public interface IDoubleAttacker
    {
        /// <summary>
        /// Does second attack and returns the damage that the attack has done
        /// </summary>
        /// <param name="baseDamage"></param>
        /// <returns>The damage done with the second attack</returns>
        int SecondAttack(int baseDamage);
    }
}
