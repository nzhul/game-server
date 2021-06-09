using System;
using GameServer.Models.Users;
using LiteNetLib;

namespace GameServer
{
    public class ServerConnection
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public int UserId { get; set; }

        public User User { get; set; }

        /// <summary>
        /// Unity connection Idl
        /// </summary>
        public int ConnectionId { get; set; }

        public int? GameId { get; set; }

        public Guid? BattleId { get; set; }

        public string Username { get; set; }

        public int MMR { get; set; }

        public string Token { get; set; }

        public NetPeer Peer { get; set; }
    }
}
