using System;

namespace GameServer.Shared.Packets.World.ClientServer
{
    [Serializable]
    public class Net_CancelFindOpponentRequest : NetMessage
    {
        public Net_CancelFindOpponentRequest()
        {
            OperationCode = NetOperationCode.CancelFindOpponentRequest;
        }
    }
}
