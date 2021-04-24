using LiteNetLib.Utils;

namespace GameServer.Shared.Packets.Users
{
    public struct Net_LogoutRequest : INetPacket
    {
        public PacketType Type => PacketType.LogoutRequest;

        public void Deserialize(NetDataReader reader)
        {
            // nothing to do here
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
