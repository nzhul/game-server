using GameServer.MapGeneration.Models;
using GameServer.Models;

namespace GameServer.MapGeneration
{
    public interface IMapGenerator
    {
        Map GenerateZone(int width = 128,
            int height = 76,
            int borderSize = 0,
            int passageRadius = 1,
            int minRoomSize = 50,
            int minWallSize = 50,
            int randomFillPercent = 47);

        Map TryGenerateZone(ZoneConfig config);

        Map TryGenerateMap(GameParams gameParams);
    }
}
