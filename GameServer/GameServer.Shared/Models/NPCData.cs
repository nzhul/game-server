using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace GameServer.Shared.Models
{
    public class NPCData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CreatureType MapRepresentation { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Disposition Disposition { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TreasureType RewardType { get; set; }

        public int RewardQuantity { get; set; }

        public object ItemReward { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CreatureType TroopsRewardType { get; set; }

        public int TroopsRewardQuantity { get; set; }

        public string OccupiedTilesString { get; set; }

        public DateTime LastDefeat { get; set; }

        public bool IsLocked { get; set; }
    }
}
