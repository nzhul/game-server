using System;

namespace GameServer.Shared.Packets.World.ClientServer
{
    [Serializable]
    public class Net_ReconnectBattleRequest : NetMessage
    {
        public Net_ReconnectBattleRequest()
        {
            OperationCode = NetOperationCode.ReconnectBattleRequest;
        }

        public int GameId { get; set; } // TODO: later on the game id should be resolved by the server.

        public Guid BattleId { get; set; } // TODO: later on the battle id should be resolved by the server.
    }
}
