using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;
using Server.Models.Games;

namespace Server.Data.Services.Implementation
{
    public class GameService : BaseService, IGameService
    {
        private readonly IMapper _mapper;


        public GameService(
            DataContext context,
            IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Game> GetGameAsync(int id)
        {
            //NOTE: AutoComplete for .ThenInclude is not working! Do not wonder why you cannot see nested entities :)

            return await _context.Games
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task EndGame(int gameId, int winnerId)
        {

            // TODO: Not implemented.
            // Change game state and record the ScoreScreen statistics
            // Increase the winner MMR
            // Lower the loosers MMR

            // get the game
            // get all players in the game
            // increase MMR of the winner
            // lower the MMR of the loosers
            // clear game reference in all players
            // mark game as completed.
            // TODO: archive/delete the game
        }
    }
}
