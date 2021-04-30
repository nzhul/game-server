using System;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Games;
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
                var movingArmy = GameManager.Instance.GetArmy(gameId, msg.ArmyId);

                // BUG: the game is not loaded because it is from reconnect!
                // Temporary hack: load all games on server start!
                // We won't need to do this in the actual game, because the game will be there.

                // 2. Notify the interested clients ( must exclude the requester )
                base.NotifyClientsInGame(gameId, rmsg);

                // 3. Update army position here in the dedicated server cache.
                base.UpdateCache(movingArmy, msg.Destination, movingArmy.GameId);

                // 5. Update the database.
                Task.Run(() =>
                {
                    try
                    {
                        RequestManagerHttp.ArmiesService.UpdateArmyPosition(msg.ArmyId, msg.Destination.X, msg.Destination.Y);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating army position. GameId: {gameId}, ArmyId: {movingArmy.Id} Ex: {ex}");
                    }
                });

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
