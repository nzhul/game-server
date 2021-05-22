using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Heroes;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Models.Armies
{
    public class Army : MapEntity
    {
        public Army()
        {
            this.Units = new Collection<Unit>();
        }

        public virtual IList<Unit> Units { get; set; }

        public int? GameId { get; set; }

        public virtual Game Game { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }


        [NotMapped]
        public Guid? Link { get; set; }
    }
}
