using Server.Models;
using Server.Models.Heroes;
using Server.Models.MapEntities;
using System;
using System.Collections.Generic;

namespace Server.Api.Models.View.Realms
{
    public class HeroDetailedDto
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public HeroType Type { get; set; }

        public string Name { get; set; }

        public DateTime LastActivity { get; set; }

        public TimeSpan TimePlayed { get; set; }

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

        public string Faction { get; set; }

        public string Class { get; set; }

        public string RegionId { get; set; }

        public NPCData NPCData { get; set; }

        public bool IsNPC { get; set; }

        public ICollection<UnitDetailedDto> Units { get; set; }
    }
}
