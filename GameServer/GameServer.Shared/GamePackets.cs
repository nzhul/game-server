using LiteNetLib.Utils;

namespace GameServer.Shared
{


    #region Packets
    public struct StartBattleRequest : INetPacket
    {
        public PacketType Type => PacketType.StartBattleRequest;

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
        public PacketType Type => PacketType.OnEndBattle;

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
        None = 0,

        #region ClientServer

        AuthRequest = 1,
        LoadRegionsRequest = 2,

        /// <summary>
        /// World enter request will force the dedicated server to load all the information about the user avatar
        /// together with the required regions
        /// </summary>
        WorldEnterRequest = 3,

        /// <summary>
        /// Request entity movement
        /// Do validation and if success -> send OnMovement message that contains the updated coordinates.
        /// </summary>
        MapMovementRequest = 4,

        /// <summary>
        /// Sends request for teleport to the dedicated server.
        /// 1. DDServer validates the request and sends back OnTeleport message.
        /// 2. Client listens for OnTeleport messages and execute the teleport.
        ///     - Cases:
        ///         1. self teleport -> the requester is the one that is teleporting
        ///         2. other player -> a player teleports in or out of our map.
        /// </summary>
        TeleportRequest = 5,

        /// <summary>
        /// Sends request for starting a battle to the dedicated server.
        /// 1. DDServer checks if the monster pack is not locked and sends back OnStartBattle message.
        /// 2. Client listens for this message and starts the battle by loading separate "battle" scene.
        /// </summary>
        StartBattleRequest = 6,

        /// <summary>
        /// Each player should send this message after he is done loading the battle scene.
        /// Then we know that the battle can start.
        /// </summary>
        ConfirmLoadingBattleScene = 7,

        /// <summary>
        /// A player that currently have an active turn can send an EndTurnRequest 
        /// to end his turn befire his time expires
        /// The player should not be able to send such request if current turn is not 
        /// his or there is X time left from his turn.
        /// </summary>
        EndTurnRequest = 8,

        /// <summary>
        /// Sends request for joining the matchmaking queue and start searching for opponent.
        /// </summary>
        FindOpponentRequest = 9,

        /// <summary>
        /// Sends a request for leaving matchmaking pool.
        /// </summary>
        CancelFindOpponentRequest = 10,

        /// <summary>
        /// Sends a request for disconnecting from the server and going offline.
        /// </summary>
        LogoutRequest = 11,

        /// <summary>
        /// Sends a request for reconnecting to a game.
        /// NOTE: This will be used only while developing so we can load the game server side when we need it.
        /// </summary>
        ReconnectRequest = 12,

        /// <summary>
        /// Sends a request for reconnecting to a battle.
        /// This message is send when the player still have active game and wants to reconnect to it.
        /// </summary>
        ReconnectBattleRequest = 13,
        #endregion


        #region ServerClient

        OnAuthRequest = 100,

        OnWorldEnter = 101,

        OnMapMovement = 102,

        OnTeleport = 103,

        OnStartBattle = 104,

        OnSwitchTurn = 105,

        OnStartGame = 106,

        OnEndBattle = 107, // TODO: Delete this.

        #endregion
    }

    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
    #endregion

}
