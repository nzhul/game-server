using System;
using LiteNetLib.Utils;

namespace GameServer.Shared.Packets.Battle
{
    public struct Net_ConfirmLoadingBattleSceneRequest : INetPacket
    {
        public PacketType Type => PacketType.ConfirmLoadingBattleScene;

        public Guid BattleId { get; set; } // TODO: Replace Guids with Int so the requests are smaller

        public int ArmyId { get; set; }

        public bool IsReady { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            BattleId = Guid.Parse(reader.GetString());
            ArmyId = reader.GetInt();
            IsReady = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(BattleId.ToString());
            writer.Put(ArmyId);
            writer.Put(IsReady);
        }
    }
}