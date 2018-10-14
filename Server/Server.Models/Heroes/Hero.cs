using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Models.Items;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Models.Heroes
{
    public class Hero : MapEntity
    {
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

        public int Level { get; set; }

        public int Attack { get; set; }

        public int Defence { get; set; }

        public int Magic { get; set; }

        public int MagicPower { get; set; }

        public int PersonalAttack { get; set; }

        public int PersonalDefense { get; set; }

        public int Dodge { get; set; }

        public int Health { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int MagicResistance { get; set; }

        public int? BlueprintId { get; set; }

        public virtual HeroBlueprint Blueprint { get; set; }

        public int? RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int? AvatarId { get; set; }

        public virtual Avatar Avatar { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Hero()
        {
            this.Items = new Collection<Item>();
        }
    }
}
