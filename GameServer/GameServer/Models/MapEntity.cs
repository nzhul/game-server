namespace GameServer.Models
{
    public abstract class MapEntity : Entity
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Team Team { get; set; }
    }
}
