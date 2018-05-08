using Server.Models.Heroes;

namespace Server.Models.Items
{
    public class Item
    {
        public int Id { get; set; }

        public int BlueprintId { get; set; }

        public ItemBlueprint Blueprint { get; set; }

        public int HeroId { get; set; }

        public Hero Hero { get; set; }
    }
}
