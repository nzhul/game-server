namespace Respawner.Configuration
{
    public interface IServiceConfiguration
    {
        string Description { get; }
        string ServiceName { get; }
        string DisplayName { get; }
    }
}
