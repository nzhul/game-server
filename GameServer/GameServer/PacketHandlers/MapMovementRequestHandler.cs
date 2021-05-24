using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using NetworkingShared;
using NetworkingShared.Attributes;
using NetworkingShared.Packets.World.ClientServer;
using NetworkingShared.Packets.World.ServerClient;
using NetworkShared.Models;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.MapMovementRequest)]
    public class MapMovementRequestHandler : MovementHandlerBase, IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            Net_MapMovementRequest msg = (Net_MapMovementRequest)packet;
            Net_OnMapMovement rmsg = new Net_OnMapMovement();


            // 1. Validate the new position
            if (IsNewPositionValid(msg))
            {
                rmsg.Success = 1;
                rmsg.ArmyId = msg.ArmyId;
                rmsg.Destination = new Coord
                {
                    X = msg.Destination.X,
                    Y = msg.Destination.Y
                };

                var gameId = GameManager.Instance.GetGameIdByConnectionId(connectionId);
                var movingArmy = GameManager.Instance.GetArmy(gameId.Value, msg.ArmyId);

                // 2. Notify the interested clients ( must exclude the requester )
                base.NotifyClientsInGame(gameId.Value, rmsg);

                // 3. Update army position here in the dedicated server cache.
                base.UpdateCache(movingArmy, msg.Destination, movingArmy.GameId);

                // Note: Both UpdateCached and UpdateDatabase is happening after client notification.
                // That is done on purpose so we do not slow down the response to the client after we know that the request is valid.
            }
            else
            {
                rmsg.Error = "Requested position is not valid!";
                rmsg.Success = 0;
                NetworkServer.Instance.Send(connectionId, rmsg);
            }
        }

        private bool IsNewPositionValid(Net_MapMovementRequest msg)
        {
            // TODO: Implement the validation
            return true;
        }
    }
}
