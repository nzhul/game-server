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
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
