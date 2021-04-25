using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.StartBattleRequest)]
    public class StartBattleRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var request = (StartBattleRequest)packet;
            System.Console.WriteLine($"[StartBattle] Attacker ({request.AttackerArmyId}) vs Defender ({request.DefenderArmyId})");
        }
    }
}
