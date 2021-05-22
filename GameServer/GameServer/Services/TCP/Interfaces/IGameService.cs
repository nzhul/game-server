namespace Assets.Scripts.Network.Services.TCP.Interfaces
{
    public interface IGameService
    {
        void StartGame(int gameId, int[] connectionIds);
    }
}
