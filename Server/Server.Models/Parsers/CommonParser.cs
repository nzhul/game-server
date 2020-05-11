using System;
using System.Collections.Generic;

namespace Server.Models.Parsers
{
    public static class CommonParser
    {
        public static List<Coord> ParseTiles(string tilesString)
        {
            List<Coord> roomCoordinates = new List<Coord>();

            string[] tilesParts = tilesString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tilesParts.Length; i++)
            {
                string[] coords = tilesParts[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                roomCoordinates.Add(new Coord() { X = int.Parse(coords[0]), Y = int.Parse(coords[1]) });
            }

            return roomCoordinates;
        }
    }
}
