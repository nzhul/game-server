using LiteNetLib.Utils;

namespace GameServer.Shared
{


    #region Packets
    public struct StartBattleRequest : INetPacket
    {
        public PacketType Type => PacketType.StartBattle;

        public int AttackerArmyId;

        public int DefenderArmyId;

        public void Deserialize(NetDataReader reader)
        {
            AttackerArmyId = reader.GetInt();
            DefenderArmyId = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(AttackerArmyId);
            writer.Put(DefenderArmyId);
        }
    }

    public struct EndBattleEvent : INetPacket
    {
        public PacketType Type => PacketType.EndBattle;

        public int BattleId;

        public void Deserialize(NetDataReader reader)
        {
            BattleId = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(BattleId);
        }
    }
    #endregion

    #region Common
    public enum PacketType : byte
    {
        StartBattle,
        EndBattle,
    }

    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
    #endregion

}
