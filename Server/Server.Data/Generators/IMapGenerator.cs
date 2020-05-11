using Server.Data.Generators.Models;
using Server.Models.Realms.Input;

namespace Server.Data.Generators
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

        Map PopulateZone(Map emptyMap, int monsterStrength, int objectDencity);

        Map TryGenerateZone(ZoneConfig config);

        Map TryGenerateMap(StartGameConfig gameConfig);
    }
}
