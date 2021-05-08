using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Models.Battle;

namespace GameServer.Managers
{
    public class BattleManager
    {
        private static BattleManager _instance;

        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }

                return _instance;
            }
        }

        private List<Battle> ActiveBattles = new List<Battle>();

        public void RegisterBattle(Battle battle)
        {
            this.ActiveBattles.Add(battle);
        }

        public Battle GetBattleById(Guid battleId)
        {
            return this.ActiveBattles.FirstOrDefault(x => x.Id == battleId);
        }
    }
}
