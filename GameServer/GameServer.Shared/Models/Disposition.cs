namespace GameServer.Shared.Models
{
    public enum Disposition
    {
        Complaint = 0, // Will always join army
        Friendly = 1, // likely to join army
        Aggressive = 2, // may join army
        Hostile = 3, // unlikely to join army
        Savage = 4 // will never join army
    }
}
