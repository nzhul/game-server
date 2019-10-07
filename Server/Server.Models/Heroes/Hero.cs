using Server.Models.Heroes.Units;
using Server.Models.Items;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Heroes
{
    /// <summary>
    /// Hero class represents both the hero itself and the whole army.
    /// A hero can be alive and roam the map together with his units
    /// A hero can be dead but still be able to roam the map and fight if there are alive units in his group
    /// A hero can be just a placeholder that acts as a wrapper for the units. Typically this is the case with neutral creatures.
    /// </summary>
    public class Hero : MapEntity
    {
        #region Attributes

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Level { get; set; }

        public int MovementPoints { get; set; }

        public int ActionPoints { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int Hitpoints { get; set; }

        public int Mana { get; set; }

        public int Armor { get; set; }

        public int Dodge { get; set; }

        public AttackType AttackType { get; set; }

        public ArmorType ArmorType { get; set; }
        #endregion

        public HeroType Type { get; set; }

        public DateTime LastActivity { get; set; }

        [Obsolete("Property 'Duration' should be used instead.")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public long TimePlayedTicks { get; set; }

        [NotMapped]
        public TimeSpan TimePlayed
        {
#pragma warning disable 618
            get
            {
                return new TimeSpan(TimePlayedTicks);
            }
            set
            {
                TimePlayedTicks = value.Ticks;
            }
#pragma warning restore 618
        }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public int? BlueprintId { get; set; }

        public virtual HeroBlueprint Blueprint { get; set; }

        public int? RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int? AvatarId { get; set; }

        public virtual Avatar Avatar { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual ICollection<Unit> Units { get; set; }

        // Additional heroes that go together with the main hero.
        // Can be max 3 additional -> total of 3 heroes in the battlefield.
        //public virtual ICollection<Hero> Followers { get; set; }

        public Hero()
        {
            this.Items = new Collection<Item>();
            this.Units = new Collection<Unit>();
        }
    }
}
