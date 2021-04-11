using System.Collections.Generic;
using GameServer.Shared;

namespace GameClient.PacketHandlers
{
    public static class HandlerRegistry
    {
        public static readonly Dictionary<PacketType, IPacketHandler> Handlers =
            new Dictionary<PacketType, IPacketHandler>
            {
                { PacketType.EndBattle, new OnEndBattleEventHandler() },
            };
    }
}
