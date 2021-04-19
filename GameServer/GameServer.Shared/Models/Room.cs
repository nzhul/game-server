namespace GameServer.Shared.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string TilesString { get; set; }

        public string EdgeTilesString { get; set; }

        public int RoomSize { get; set; }

        public bool IsMainRoom { get; set; }

        public bool IsAccessibleFromMainRoom { get; set; }
    }
}
