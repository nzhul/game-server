using System.Collections.Generic;
using GameServer.Models;

namespace GameServer.MapGeneration.Models
{
    public class ZoneConfig
    {
        public int Id { get; set; }

        public int Y { get; set; }

        public int X { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ZoneType Type { get; set; }

        public Team Team { get; set; }

        public TerrainType TerrainType { get; set; }

        public int Fuzziness { get; set; }

        //public MinesConfig MinesConfig { get; set; }

        //public MonstersConfig MonstersConfig { get; set; }

        public ICollection<ZoneLink> Links { get; set; }
    }

    public class ZoneLink
    {
        public int ZoneId { get; set; }

        public int GuardianStrength { get; set; }


    }

    public enum ZoneType
    {
        StartingZone,
        NeutralZone
    }

    public enum TerrainType
    {
        MatchToTown,
        Grass,
        Lava,
        Subterranean,
        Unholy
    }
}
