using System;
using GameServer.Shared.Models;
using LiteNetLib.Utils;

namespace GameServer.Shared.Packets.Battle
{

    public struct Net_SwitchTurnEvent : INetPacket
    {
        public PacketType Type => PacketType.OnSwitchTurn;

        public Guid BattleId { get; set; }

        public int CurrentUnitId { get; set; }

        public Turn Turn { get; set; }


        public void Deserialize(NetDataReader reader)
        {
            BattleId = Guid.Parse(reader.GetString());
            CurrentUnitId = reader.GetInt();
            Turn = (Turn)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(BattleId.ToString());
            writer.Put(CurrentUnitId);
            writer.Put((byte)Turn);
        }
    }
}
