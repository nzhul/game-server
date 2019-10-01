using Server.Models;
using Server.Models.MapEntities;

namespace Server.Api.Models.View.Realms
{
    public class UnitDetailedDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Id of the owner avatar
        /// </summary>
        public int OwnerId { get; set; }

        public int RegionId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public CreatureType CreatureType { get; set; }

        public int Quantity { get; set; }
    }
}
