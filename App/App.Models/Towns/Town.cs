using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Towns
{
    public class Town : ITown
    {

        private ICollection<Building> buildings;

        public Town()
        {
            this.buildings = new HashSet<Building>();
        }

        public int Id { get; set; }

        public virtual AppUser Owner { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Building> Buildings 
        {
            get { return this.buildings; }
            set { this.buildings = value;  }
        }
    }
}
