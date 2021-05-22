namespace GameServer.Shared.Models
{
    public enum TreasureType
    {
        Wood = 0,
        Stone = 1,
        Gold = 2,
        Experience = 4, // This is direct experience boost in addition to the experience gained after battle.
        TreasureChest = 5,
        Troops = 6,
        Artifact = 7
    }
}
