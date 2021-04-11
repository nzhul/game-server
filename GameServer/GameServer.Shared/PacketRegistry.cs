using System;
using System.Collections.Generic;

namespace GameServer.Shared
{
    public static class PacketRegistry
    {
        public static readonly Dictionary<PacketType, Type> PacketTypes =
            new Dictionary<PacketType, Type>
            {
                { PacketType.StartBattle, typeof(StartBattleRequest) },
                { PacketType.EndBattle, typeof(EndBattleEvent) }
            };
    }
}
