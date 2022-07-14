using System;
using Phatra.Core.Configuration;
using Phatra.Core.Infrastructure.DependencyManagement;

namespace Phatra.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the 
    /// various services composing the Phatra engine. Edit functionality, modules
    /// and implementations access most Phatra functionality through this 
    /// interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Container manager
        /// </summary>
        IContainerManager ContainerManager { get; }

        /// <summary>
        /// Initialize components and plugins in the Phatra environment.
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(IEngineConfig config);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
