using Server.Models.Heroes;

namespace Server.Models.Items
{
    public class Item : Entity
    {
        public int? BlueprintId { get; set; }

        public ItemBlueprint Blueprint { get; set; }

        public int? HeroId { get; set; }

        public Unit Unit { get; set; }
    }
}
