namespace GameServer.Shared
{
    public interface IPacketHandler
    {
        void Handle(INetPacket packet);
    }
}
