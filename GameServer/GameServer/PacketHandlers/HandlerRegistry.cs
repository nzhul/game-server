using System.Collections.Generic;
using GameServer.Shared;

namespace GameServer.PacketHandlers
{
    // TODO: Replace this whole class with Reflection!
    public static class HandlerRegistry
    {
        public static readonly Dictionary<PacketType, IPacketHandler> Handlers =
            new Dictionary<PacketType, IPacketHandler>
            {
                { PacketType.StartBattleRequest, new StartBattleRequestHandler() },
                { PacketType.ConfirmLoadingBattleScene, new ConfirmLoadingBattleSceneHandler() },
                { PacketType.EndTurnRequest, new EndTurnRequestHandler() },
            };
    }
}
