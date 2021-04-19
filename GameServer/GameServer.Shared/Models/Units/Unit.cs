using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameServer.Shared.Models.Units
{
    public class Unit
    {
        public int Id { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Level { get; set; }

        public int? GameId { get; set; }

        public int? AvatarId { get; set; }

        public int? ArmyId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CreatureType Type { get; set; }

        public int Quantity { get; set; }


        #region Non-Persistend properties

        // The non-persistend properties will not be stored in database and won't be unique per unit
        // Instead there will be special UnitConfigurations table in the database where we will store configuration for each type of unit
        // For example:
        // Bowman:
        // BaseMovePoints: 2
        // BaseActionPoints: 1
        // MinDamage: 10
        // MaxDamage: 15
        // ...
        // Those properties will be loaded on server start.

        public int MovementPoints { get; set; }

        public int MaxMovementPoints { get; set; }

        public int ActionPoints { get; set; }

        public int MaxActionPoints { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        /// <summary>
        /// Current hitpoints of the creature.
        /// </summary>
        public int Hitpoints { get; set; }

        /// <summary>
        /// Hitpoints of the creature at the start of the battle -> this.Config.BaseHitpoints + Upgrades
        /// </summary>
        public int BaseHitpoints { get; set; }

        /// <summary>
        /// Current Max hitpoints of the creature -> this.BaseHitpoints + Buffs/Debufs
        /// </summary>
        public int MaxHitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        public int Dodge { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttackType AttackType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ArmorType ArmorType { get; set; }

        public bool ActionConsumed { get; set; }

        #endregion
    }
}
