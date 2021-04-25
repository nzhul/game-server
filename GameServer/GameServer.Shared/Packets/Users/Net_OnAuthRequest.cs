using GameServer.Shared;
using LiteNetLib.Utils;

namespace Assets.Scripts.Network.Shared.NetMessages.Users
{
    public struct Net_OnAuthRequest : INetPacket
    {
        public PacketType Type => PacketType.OnAuthRequest;

        public int ConnectionId { get; set; }

        public byte Success { get; set; }

        public string ErrorMessage { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            ConnectionId = reader.GetInt();
            Success = reader.GetByte();
            ErrorMessage = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(ConnectionId);
            writer.Put(Success);
            writer.Put(ErrorMessage);
        }
    }
}
