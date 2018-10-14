namespace Server.Models.MapEntities
{
    public enum Disposition
    {
        Complaint = 0, // Will always join hero
        Friendly = 1, // likely to join hero
        Aggressive = 2, // may join hero
        Hostile = 3, // unlikely to join hero
        Savage = 4 // will never join hero
    }
}
