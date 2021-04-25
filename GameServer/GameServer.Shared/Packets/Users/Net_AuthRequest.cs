using System;
using GameServer.Shared;
using LiteNetLib.Utils;

namespace Assets.Scripts.Network.Shared.NetMessages.Users
{
    // TODO: AuthRequest should not pass all this information.
    // Server should do another request to the server to fetch user details like:
    // MMR, GameId, BattleId etc.
    public struct Net_AuthRequest : INetPacket
    {
        public PacketType Type => PacketType.AuthRequest;

        public int UserId { get; set; }

        public string Username { get; set; }

        public int MMR { get; set; }

        public string Token { get; set; }

        public int GameId { get; set; }

        public Guid? BattleId { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            UserId = reader.GetInt();
            Username = reader.GetString();
            MMR = reader.GetInt();
            Token = reader.GetString();
            GameId = reader.GetInt();

            if (reader.TryGetString(out string battleIdString))
            {
                BattleId = Guid.Parse(battleIdString);
            };
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(UserId);
            writer.Put(Username);
            writer.Put(MMR);
            writer.Put(Token);
            writer.Put(GameId);
            if (BattleId.HasValue)
            {
                writer.Put(BattleId.Value.ToString());
            }
        }

        public bool IsValid()
        {
            bool result = true;

            if (this.UserId == 0)
            {
                result = false;
            }

            if (string.IsNullOrEmpty(this.Username))
            {
                result = false;
            }

            if (string.IsNullOrEmpty(this.Token))
            {
                result = false;
            }

            return result;
        }
    }
}