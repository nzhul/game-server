using System;

namespace GameServer.Shared.NetMessages.Users
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
