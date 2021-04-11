using GameServer.Shared;

namespace GameClient.PacketHandlers
{
    public class OnEndBattleEventHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var @event = (EndBattleEvent)packet;
            System.Console.WriteLine($"[EndBattle] BattleId: {@event.BattleId}");
        }
    }
}
