using System;

namespace GameServer.Shared.NetMessages.World.ClientServer
{
    [Serializable]
    public class Net_ReconnectRequest : NetMessage
    {
        public Net_ReconnectRequest()
        {
            OperationCode = NetOperationCode.ReconnectRequest;
        }

        public int GameId { get; set; }
    }
}
