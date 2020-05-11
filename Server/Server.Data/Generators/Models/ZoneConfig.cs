using System.Collections.Generic;

namespace Server.Data.Generators.Models
{
    public class ZoneConfig
    {
        public int Id { get; set; }

        public int Y { get; set; }

        public int X { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ZoneType Type { get; set; }

        public TerrainType TerrainType { get; set; }

        public int Fuzziness { get; set; }

        //public MinesConfig MinesConfig { get; set; }

        //public MonstersConfig MonstersConfig { get; set; }

        public ICollection<ZoneLink> Links { get; set; }
    }

    public class ZoneLink
    {
        public int ZoneId { get; set; }

        public int GuardiansStrength { get; set; }
    }

    public enum ZoneType
    {
        HumanStart,
        ComputerStart,
        Treasure
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
