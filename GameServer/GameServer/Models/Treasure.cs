namespace GameServer.Models
{
    public class Treasure : MapEntity
    {
        public TreasureType Type { get; set; }

        public int Quantity { get; set; }
    }
}
