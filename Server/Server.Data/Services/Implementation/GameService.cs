using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data.Generators;
using Server.Data.Services.Abstraction;
using Server.Models.Heroes;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;

namespace Server.Data.Services.Implementation
{
    public class GameService : BaseService, IGameService
    {
        private readonly IMapper _mapper;

        private readonly IMapGenerator _mapGenerator;

        public GameService(
            DataContext context,
            IMapGenerator mapGenerator,
            IMapper mapper)
            : base(context)
        {
            _mapGenerator = mapGenerator;
            _mapper = mapper;
        }

        public async Task<Game> GetGameAsync(int id)
        {
            //NOTE: AutoComplete for .ThenInclude is not working! Do not wonder why you cannot see nested entities :)

            return await _context.Games
                .Include(r => r.Heroes)
                .Include(r => r.Rooms)
                .Include(r => r.Dwellings)
                .Include(r => r.Avatars).ThenInclude(c => c.Heroes)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Game> CreateGameAsync(GameParams gameParams)
        {
            var avatars = this.InitAvatars(gameParams);
            Map generatedMap = _mapGenerator.TryGenerateMap(gameParams);

            var newGame = new Game()
            {
                Name = "G_" + DateTime.UtcNow.Ticks.ToString(),
                MatrixString = generatedMap.MatrixString,
                Rooms = this.ConvertToRooms(generatedMap.Rooms),
                Dwellings = generatedMap.Dwellings,
                Treasures = generatedMap.Treasures,
                Heroes = generatedMap.Heroes,
                Avatars = avatars
            };

            this.AssignAvatarsToEntities(avatars, newGame);

            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();

            await this.AssignGameToUsers(newGame.Id, gameParams.Players.Select(x => x.UserId));

            return newGame;
        }

        private async Task AssignGameToUsers(int gameId, IEnumerable<int> userIds)
        {
            var users = _context.Users.Where(x => userIds.Contains(x.Id));
            await users.ForEachAsync(x => { x.ActiveGameId = gameId; });
            await _context.SaveChangesAsync();
        }

        private void AssignAvatarsToEntities(ICollection<Avatar> avatars, Game newGame)
        {
            var teams = (Team[])Enum.GetValues(typeof(Team));

            for (int i = 1; i < teams.Length; i++)
            {
                var team = teams[i];
                var avatarsFromTeam = avatars.Where(x => x.Team == team);
                var heroes = newGame.Heroes.Where(x => x.Team == team && x.Type == HeroType.Normal && x.Avatar == null);
                var dwellings = newGame.Dwellings.Where(x => x.Team == team && x.Owner == null);

                foreach (var avatar in avatarsFromTeam)
                {
                    var availibleHero = heroes.FirstOrDefault(x => x.Team == avatar.Team);
                    var availibleCastle = dwellings.FirstOrDefault(x => x.Team == avatar.Team
                        && x.Type == DwellingType.Castle && x.Link == availibleHero.Link);

                    availibleHero.Avatar = avatar;
                    availibleCastle.Owner = avatar;
                    //TODO: we are currently handling only heroes and castles. Handle other dwellings if needed.
                }
            }
        }

        private ICollection<Room> ConvertToRooms(List<TempRoom> rooms)
        {
            var dbRooms = new List<Room>();
            foreach (var room in rooms)
            {
                var newRoom = new Room()
                {
                    TilesString = MapGenerator.StringifyCoordCollection(room.tiles),
                    EdgeTilesString = MapGenerator.StringifyCoordCollection(room.edgeTiles),
                    RoomSize = room.roomSize,
                    IsMainRoom = room.isMainRoom,
                    IsAccessibleFromMainRoom = room.isAccessibleFromMainRoom
                };

                dbRooms.Add(newRoom);
            }

            return dbRooms;
        }

        private ICollection<Avatar> InitAvatars(GameParams gameParams)
        {
            return gameParams.Players
                .Select(x => new Avatar
                {
                    UserId = x.UserId,
                    Team = x.Team
                })
                .ToList();
        }

        private (int height, int width) GetDimentions(int mapSize)
        {
            int height;
            int width;

            switch (mapSize)
            {
                case 1:
                    width = 50;
                    height = 30;
                    break;
                case 2:
                    width = 70;
                    height = 50;
                    break;
                case 3:
                    width = 100;
                    height = 70;
                    break;
                case 4:
                    width = 140;
                    height = 90;
                    break;
                default:
                    width = 50;
                    height = 30;
                    break;
            }

            return (height, width);
        }
    }
}
