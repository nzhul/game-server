using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameClient.PacketHandlers
{
    [HandlerRegister(PacketType.OnEndBattle)]
    public class OnEndBattleEventHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var @event = (EndBattleEvent)packet;
            System.Console.WriteLine($"[EndBattle] BattleId: {@event.BattleId}");
        }
    }
}
