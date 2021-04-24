using System;

namespace Assets.Scripts.Network.Shared.NetMessages.World.ClientServer
{
    [Serializable]
    public class Net_TeleportRequest : NetMessage
    {
        public Net_TeleportRequest()
        {
            OperationCode = NetOperationCode.TeleportRequest;
        }

        public int ArmyId { get; set; }

        public int GameId { get; set; }

        public int DwellingId { get; set; }
    }
}