using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Items;
using Server.Models.Parsers;

namespace Server.Models.MapEntities
{
    public class MonsterPack : MapEntity
    {
        public MonsterType Type { get; set; }

        public int Quantity { get; set; }

        public Disposition Disposition { get; set; }

        /// <summary>
        /// if reward type is troop or artifact.
        /// use ItemReward or TroopsRewardType to create new item/monster and assign it to the hero.
        /// </summary>
        public TreasureType RewardType { get; set; }

        public int RewardQuantity { get; set; }

        /// <summary>
        /// This will will have value only when RewardType = TreasureType.Artifact
        /// </summary>
        public ItemBlueprint ItemReward { get; set; }

        public MonsterType TroopsRewardType { get; set; }

        public int TroopsRewardQuantity { get; set; }

        private string _occupiedTilesString;

        public string OccupiedTilesString
        {
            get
            {
                return this._occupiedTilesString;
            }
            set
            {
                if (value != null)
                {
                    this._occupiedTilesString = value;
                    this.OccupiedTiles = CommonParser.ParseTiles(this._occupiedTilesString);
                }
            }
        }

        [NotMapped]
        public List<Coord> OccupiedTiles { get; set; }
    }
}
