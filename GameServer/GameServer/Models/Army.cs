using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameServer.Models.Units;
using GameServer.Models.Users;
using Newtonsoft.Json;

namespace GameServer.Models
{
    public class Army : MapEntity
    {
        public Army()
        {
            this.Units = new Collection<Unit>();
        }

        public int? UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public Avatar Avatar { get; set; }

        public int GameId { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public IList<Unit> Units { get; set; }

        public Guid? Link { get; set; }

        public bool TurnConsumed { get; set; }

        public int Order { get; set; }

        private bool _readyForBattle;

        public bool ReadyForBattle 
        { 
            get 
            {
                if (IsNPC)
                {
                    return true;
                }

                return _readyForBattle;
            }

            set 
            {
                _readyForBattle = value;
            } 
        }

        public DateTime LastActivity { get; set; }
    }
}
