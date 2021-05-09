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

        private List<Battle> _activeBattles = new List<Battle>();

        public void RegisterBattle(Battle battle)
        {
            this._activeBattles.Add(battle);
        }

        public Battle GetBattleById(Guid battleId)
        {
            return this._activeBattles.FirstOrDefault(x => x.Id == battleId);
        }

        public List<Battle> GetBattles()
        {
            return _activeBattles;
        }

        public void EndBattle(Battle battle, int winnerId)
        {
            _activeBattles.Remove(battle);
        }
    }
}
