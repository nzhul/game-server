namespace Server.Models
{
    public abstract class MapEntity : Entity
    {
        public string Name { get; set; }

        public int X { get; set; }

        public int Z { get; set; }
    }
}
