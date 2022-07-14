namespace Phatra.Core.Configuration
{
    public interface IEngineConfig
    {
        /// <summary>
        /// In addition to configured assemblies examine and load assemblies in the bin directory.
        /// </summary>
        bool DynamicDiscovery { get;}

        /// <summary>
        /// A custom <see cref="IEngine"/> to manage the application instead of the default.
        /// </summary>
        string EngineType { get; }

        /// <summary>
        /// Indicates whether we should ignore startup tasks
        /// </summary>
        bool IgnoreStartupTasks { get; }
    }
}
