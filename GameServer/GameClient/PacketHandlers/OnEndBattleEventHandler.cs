using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameClient.PacketHandlers
{
    [HandlerRegister(PacketType.OnEndBattle)]
    public class OnEndBattleEventHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var @event = (EndBattleEvent)packet;
            System.Console.WriteLine($"[EndBattle] BattleId: {@event.BattleId}");
        }
    }
}
