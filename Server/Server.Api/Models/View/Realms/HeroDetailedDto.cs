using Server.Models.Heroes;
using Server.Models.Heroes.Units;
using Server.Models.MapEntities;
using System;
using System.Collections.Generic;

namespace Server.Api.Models.View.Realms
{
    public class HeroDetailedDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Id of the owner avatar
        /// </summary>
        public int? OwnerId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

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

        public HeroType HeroType { get; set; }

        public AttackType AttackType { get; set; }

        public ArmorType ArmorType { get; set; }

        public HeroClass HeroClass { get; set; }

        public int? GameId { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public ICollection<UnitDetailedDto> Units { get; set; }
    }
}
