using System;
using Assets.Scripts.Network.NetworkShared.Packets.World.ClientServer;
using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.RestRequest)]
    public class RestRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_RestRequest)packet;
            Console.WriteLine($"Received {nameof(msg)} packet!");

            var connection = NetworkServer.Instance.Connections[connectionId];
            connection.User.Avatar.IsResting = true;
            GameManager.Instance.DoNewDayRestCheck(connection.UserId);

            // 1. Mark avatar.TurnConsumed = true
            // 2. foreach all avatars in game and check if all TurnConsumed = true
            // 3. If true -> raise NewDay event.
        }
    }
}
