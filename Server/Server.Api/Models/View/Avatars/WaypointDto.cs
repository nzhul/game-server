namespace Server.Api.Models.View.Avatars
{
    public class WaypointDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public int RegionLevel { get; set; }
    }
}
