using GameServer.Shared.Models.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameServer.Shared.Models
{
    public class UnitConfiguration
    {
        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CreatureType Type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HeroFaction Faction { get; set; }

        public int MovementPoints { get; set; }

        public int ActionPoints { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Hitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttackType AttackType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ArmorType ArmorType { get; set; }

        public int CreatureLevel { get; set; }

        public int BuildTime { get; set; }

        public int WoodCost { get; set; }

        public int OreCost { get; set; }

        public int GoldCost { get; set; }

        public int GemsCost { get; set; }

        public int FoodCost { get; set; }
    }
}
