using System.Collections.Generic;
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
    }
}
