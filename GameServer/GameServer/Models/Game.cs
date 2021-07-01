using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameServer.Models
{
    public class Game
    {
        public Game()
        {
            StartTime = DateTime.UtcNow;
            CurrentDayStartTime = DateTime.UtcNow;
            TotalDays = 1;
            Week = 1;
            Month = 1;
        }

        public int Id { get; set; }

        public IList<Army> Armies { get; set; }

        public IList<Dwelling> Dwellings { get; set; }

        public IList<Treasure> Treasures { get; set; }

        public IList<Avatar> Avatars { get; set; }

        public DateTime StartTime { get; set; }

        public int Day { get; private set; }

        public int Week { get; private set; }

        public int Month { get; private set; }

        private int _totalDays;

        public int TotalDays
        {
            get { return _totalDays; }
            set
            {
                if (value <= _totalDays)
                {
                    throw new ArgumentException("Only increment is allowed!");
                }

                Day++;
                if (Day == 8)
                {
                    Day = 1;
                    Week++;
                }

                if (Week == 5)
                {
                    Week = 1;
                    Month += 1;
                }

                _totalDays = value;
            }
        }


        public DateTime CurrentDayStartTime { get; set; }

        // We will increase this time every time the game is paused or both players are in PvP battle
        // PauseTime is increased while Game.TimerStopped = true
        public TimeSpan PauseTime { get; set; }

        public bool TimerStopped { get; set; }

        private string _matrixString;

        public string MatrixString
        {
            get
            {
                return this._matrixString;
            }
            set
            {
                if (value != null)
                {
                    this._matrixString = value;
                    this.Matrix = this.ParseMatrix(this._matrixString);
                }
            }
        }

        [JsonIgnore]
        public int[,] Matrix { get; private set; }

        private int[,] ParseMatrix(string matrixString)
        {
            string[] lines = matrixString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            int height = lines.Length;
            int width = lines[0].Length;

            int[,] parsedMatrix = new int[height, width];
            for (int y = 0; y < height; y++)
            {
                string line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    parsedMatrix[y, x] = (int)char.GetNumericValue(line[x]);
                }
            }

            return parsedMatrix;
        }
    }
}
