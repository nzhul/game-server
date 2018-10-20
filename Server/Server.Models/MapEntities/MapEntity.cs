namespace Server.Models.MapEntities
{
    /// <summary>
    /// Entity contact point is his X:Z
    /// </summary>
    public abstract class MapEntity : Entity
    {
        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        // contact point -> Coord
        // occupied/blocked points -> List<Coord>
        // bool IsLocked
        // int LockedBy -> UserId
    }
}
