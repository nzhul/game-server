using System;
using GameServer.Shared.Models;

namespace GameServer.Shared.Packets.World.ClientServer
{
    [Serializable]
    public class Net_MapMovementRequest : NetMessage
    {
        public Net_MapMovementRequest()
        {
            OperationCode = NetOperationCode.MapMovementRequest;
        }

        public int ArmyId { get; set; }

        //public int GameId { get; set; }

        //public int NewX { get; set; }

        //public int NewY { get; set; }

        public Coord Destination { get; set; }
    }
}


//[Serializable]
//public class Net_WorldEnterRequest : NetMessage
//{
//    public Net_WorldEnterRequest()
//    {
//        OperationCode = NetOperationCode.WorldEnterRequest;
//    }

//    public int UserId { get; set; }

//    public int CurrentRealmId { get; set; }

//    public int[] RegionsForLoading { get; set; }
//}