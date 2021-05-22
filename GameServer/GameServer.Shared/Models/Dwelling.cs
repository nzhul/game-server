namespace GameServer.Shared.Models
{
    public class Dwelling
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Team Team { get; set; }

        public DwellingType Type { get; set; }

        public int? UserId { get; set; }

        public int GameId { get; set; }

        public int? GuardianId { get; set; }

        public string OccupiedTilesString { get; set; }

        public string VisitorsString { get; set; }
    }
}
