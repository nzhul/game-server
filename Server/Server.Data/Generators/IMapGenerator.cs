namespace Server.Data.Generators
{
    public interface IMapGenerator
    {
        Map GenerateMap(int width = 128,
            int height = 76,
            int borderSize = 5,
            int passageRadius = 1,
            int minRoomSize = 50,
            int minWallSize = 50,
            int randomFillPercent = 47);
    }
}
