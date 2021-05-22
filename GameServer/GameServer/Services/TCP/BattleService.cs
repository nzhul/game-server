//using System;
//using Assets.Scripts.Network.Services.TCP.Interfaces;
//using GameServer;
//using GameServer.Managers;
//using GameServer.Models.Battle;
//using NetworkingShared.Packets.Battle;
//using NetworkShared.Enums;

//namespace Assets.Scripts.Network.Services.TCP
//{
//    public class BattleService : IBattleService
//    {
//        // TODO: Move this to BattleManager. Leave here only the TCP calls
//        public void EndBattle(Battle battle, int winnerId)
//        {
//            if (winnerId <= 0)
//            {
//                // both players are loosers
//                // send two defeat requests
//            }

//            if (battle.AttackerArmy.Id == winnerId)
//            {
//                // attacker is winner
//                // defender is defeated
//                // send win to attacker and defeat to defender
//            }
//            else if (battle.DefenderArmy.Id == winnerId)
//            {
//                // attacker is defeated
//                // defender is winner
//                // send defeat to attacker and win to defender
//            }

//            // DO API call to save battle outcome

//            // TODO: Clear both users - BattleId
//        }
//    }
//}
