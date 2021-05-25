namespace Server.Models.Games
{
    public class Game : Entity
    {
        public int? WinnerId { get; set; }

        // TODO: 
        // Add SCORE SCREEN statistics like:
        // Units killed,
        // Gold/Resources gathered
        // Game Length in minutes
        // ETC.

    }
}