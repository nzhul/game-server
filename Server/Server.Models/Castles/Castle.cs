using Server.Models.Realms;

namespace Server.Models.Castles
{
    public class Castle
    {
        public int Id { get; set; }

        public int? BlueprintId { get; set; }

        public CastleBlueprint Blueprint { get; set; }

        public int? RegionId { get; set; }

        public Game Region { get; set; }
    }
}
