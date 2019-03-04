using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [NotMapped]
    public class Coord
    {

        public Coord()
        {
        }

        public Coord(int x, int y)
        {
            this.Row = x;
            this.Col = y;
        }

        public int Row { get; set; }

        public int Col { get; set; }
    }
}
