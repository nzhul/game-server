using System;

namespace GameServer.Shared.NetMessages.World.ServerClient
{
    [Serializable]
    public class Net_OnStartGame : NetMessage
    {
        public Net_OnStartGame()
        {
            OperationCode = NetOperationCode.OnStartGame;
        }

        public int GameId { get; set; }
    }
}
