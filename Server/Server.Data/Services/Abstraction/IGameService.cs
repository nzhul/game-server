using System.Threading.Tasks;
using Server.Models.Games;

namespace Server.Data.Services.Abstraction
{
    public interface IGameService : IService
    {
        Task<Game> GetGameAsync(int id);

        Task EndGame(int gameId, int winnerId);
    }
}
