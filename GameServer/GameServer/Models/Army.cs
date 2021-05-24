using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameServer.Models.Units;

namespace GameServer.Models
{
    public class Army : MapEntity
    {
        public Army()
        {
            this.Units = new Collection<Unit>();
        }

        public int? UserId { get; set; }

        // TODO: Delete if not needed
        public int GameId { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public IList<Unit> Units { get; set; }

        public Guid? Link { get; set; }
    }
}
