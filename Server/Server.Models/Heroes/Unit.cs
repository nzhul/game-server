using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Armies;
using Server.Models.Items;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Models.Heroes
{
    /// <summary>
    /// Every controllable in battle object is Unit.
    /// Units and Heroes are represented by the same entity
    /// Difference between unit and hero is in the following:
    /// - Level
    /// - Availible upgrades,
    /// - Availible abilities
    /// The difference between hero and unit is that the hero have the ability to gain levels and aquire new abilities and upgrades.
    /// Units can do the same but in rare occasions.
    /// </summary>
    public class Unit : Entity // TODO: Rename to UNIT
    {
        /// <summary>
        /// The UNIT/HERO entity won't contain any specific data about his Hitpoints/ManaPoints etc.
        /// They will be calculated at runtime based on his level and CreatureType and his UnitConfiguration
        /// NOTE: Maybe an exception to that might be "CurrentHP", "CurrentMana".
        /// </summary>

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Level { get; set; }

        public int Quantity { get; set; }

        public CreatureType Type { get; set; }

        public int? ArmyId { get; set; }

        public virtual Army Army { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Unit()
        {
            this.Items = new Collection<Item>();
        }
    }
}
