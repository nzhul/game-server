using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Server.Data.Generators;
using Server.Data.Services.Abstraction;
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
                .Include(r => r.Armies).ThenInclude(c => c.Units)
                .Include(r => r.Rooms)
                .Include(r => r.Dwellings)
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Game> CreateGameAsync(GameParams gameParams)
        {
            var users = this.InitUserAvatars(gameParams);
            Map generatedMap = _mapGenerator.TryGenerateMap(gameParams);

            var newGame = new Game()
            {
                Name = "G_" + DateTime.UtcNow.Ticks.ToString(),
                MatrixString = generatedMap.MatrixString,
                Rooms = this.ConvertToRooms(generatedMap.Rooms),
                Dwellings = generatedMap.Dwellings,
                Treasures = generatedMap.Treasures,
                Armies = generatedMap.Armies,
                Users = users
            };

            this.AssignAvatarsToEntities(users, newGame);

            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();

            await this.AssignGameToUsers(newGame.Id, gameParams.Players.Select(x => x.UserId));

            return newGame;
        }

        private async Task AssignGameToUsers(int gameId, IEnumerable<int> userIds)
        {
            var users = _context.Users.Where(x => userIds.Contains(x.Id));
            await users.ForEachAsync(x => { x.GameId = gameId; });
            await _context.SaveChangesAsync();
        }

        private void AssignAvatarsToEntities(ICollection<User> users, Game newGame)
        {
            var teams = (Team[])Enum.GetValues(typeof(Team));

            for (int i = 1; i < teams.Length; i++)
            {
                var team = teams[i];
                var usersFromTeam = users.Where(x => x.Avatar.Team == team);
                var armies = newGame.Armies.Where(x => x.Team == team && !x.IsNPC && x.User == null);
                var dwellings = newGame.Dwellings.Where(x => x.Team == team && x.User == null);

                foreach (var user in usersFromTeam)
                {
                    var availibleArmy = armies.FirstOrDefault(x => x.Team == user.Avatar.Team);
                    var availibleCastle = dwellings.FirstOrDefault(x => x.Team == user.Avatar.Team
                        && x.Type == DwellingType.Castle && x.Link == availibleArmy.Link);

                    availibleArmy.User = user;
                    availibleCastle.User = user;
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

        private ICollection<User> InitUserAvatars(GameParams gameParams)
        {
            // 1. Load the users from the database.
            // 2. Assign to each of them - new instance of Avatar

            var userIds = gameParams.Players.Select(x => x.UserId);
            var users = _context.Users.Where(x => userIds.Contains(x.Id));
            users.ForEachAsync(x =>
            {
                x.Avatar = new Avatar 
                {
                    Wood = 20,
                    Ore = 20,
                    Gold = 100,
                    Gems = 0,
                    Team = gameParams.Players.FirstOrDefault(p => p.UserId == x.Id).Team,
                    VisitedString = string.Empty
                };
            });

            return users.ToList();
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
