using App.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Towns
{
    public abstract class Building : IBuilding
    {

        public Building()
        {
            this.ResourceCosts = new Resources();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Resources ResourceCosts { get; set; }

        public string Description { get; set; }

        public virtual Town Town { get; set; }

        public int Level { get; set; }

        public bool IsUpgrading { get; set; }

        public int BuildTimeInSeconds { get; set; }

        public DateTime? BuildStartDate { get; set; }

        [NotMapped]
        public DateTime? BuildEndDate
        {
            get
            {
                TimeSpan buildTimeDuration = TimeSpan.FromSeconds(this.BuildTimeInSeconds);
                return this.BuildStartDate + buildTimeDuration;
            }
        }

    }
}
