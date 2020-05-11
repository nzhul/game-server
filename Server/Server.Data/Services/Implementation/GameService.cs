using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Server.Data.Generators;
using Server.Data.Generators.Models;
using Server.Data.Services.Abstraction;
using Server.Models.Realms;
using Server.Models.Realms.Input;
using Server.Models.Users;

namespace Server.Data.Services.Implementation
{
    public class GameService : BaseService, IGameService
    {
        private readonly IMapper _mapper;

        private readonly IMapGenerator _mapGenerator;

        public GameService(DataContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Game> StartGame(StartGameConfig gameData)
        {
            var avatars = this.InitAvatars(gameData);
            (int height, int width) = this.GetDimentions(gameData.MapSize);
            var zoneConfig = new ZoneConfig() { }; // TODO: Load from file.
            Map generatedMap = _mapGenerator.TryGenerateZone(zoneConfig); // width and height are swapped on purpose.

            var newGame = new Game()
            {
                Name = "G_" + DateTime.UtcNow.Ticks.ToString(),
                MatrixString = generatedMap.MatrixString,
                Rooms = this.ConvertToRooms(generatedMap.Rooms),
                Dwellings = generatedMap.Dwellings,
                Treasures = generatedMap.Treasures,
                InitialHeroPosition = generatedMap.InitialHeroPosition,
                Avatars = avatars
            };

            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();

            return newGame;
        }

        private ICollection<Room> ConvertToRooms(List<TempRoom> rooms)
        {
            throw new NotImplementedException();
        }

        private ICollection<Avatar> InitAvatars(StartGameConfig gameData)
        {
            return gameData.Players
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
