using System;

namespace GameServer.Shared.Packets.Users
{
    [Serializable]
    public class Net_LogoutRequest : NetMessage
    {
        public Net_LogoutRequest()
        {
            OperationCode = NetOperationCode.LogoutRequest;
        }
    }
}
