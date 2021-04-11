using GameServer.Shared;

namespace GameServer.PacketHandlers
{
    public class StartBattleRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var request = (StartBattleRequest)packet;
            System.Console.WriteLine($"[StartBattle] Attacker ({request.AttackerArmyId}) vs Defender ({request.DefenderArmyId})");
        }
    }
}
