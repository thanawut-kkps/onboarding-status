using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using Phatra.Core.Configuration;

namespace Phatra.Core.Infrastructure
{
    public abstract class BaseContext
    {               

        #region Utilities

        /// <summary>
        /// Creates a factory instance and adds a http application injecting facility.
        /// </summary>
        /// <param name="config">Config</param>
        /// <returns>New engine instance</returns>
        protected IEngine CreateEngineInstance(IEngineConfig config)
        {
            if (config != null && !string.IsNullOrEmpty(config.EngineType))
            {
                var engineType = Type.GetType(config.EngineType);
                if (engineType == null)
                    throw new ConfigurationErrorsException("The type '" + config.EngineType + "' could not be found. Please check the configuration at /configuration/nop/engine[@engineType] or check for missing assemblies.");
                if (!typeof(IEngine).IsAssignableFrom(engineType))
                    throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Nop.Core.Infrastructure.IEngine' and cannot be configured in /configuration/nop/engine[@engineType] for that purpose.");
                return Activator.CreateInstance(engineType) as IEngine;
            }

            return CreateDefaultEngine();
        }

        protected abstract IEngine CreateDefaultEngine();
       

        #endregion

        #region Methods

        private IEngineConfig _config;

        public void SetPhatraConfig(IEngineConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Initializes a static instance of the Nop factory.
        /// </summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                if (_config == null)
                {
                    _config = ConfigurationManager.GetSection("engineConfig") as IEngineConfig;
                }
                Singleton<IEngine>.Instance = CreateEngineInstance(_config);
                Singleton<IEngine>.Instance.Initialize(_config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton engine used to access services.
        /// </summary>
        public IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
