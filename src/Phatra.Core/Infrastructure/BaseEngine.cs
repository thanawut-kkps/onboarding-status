using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Phatra.Core.Configuration;
using Phatra.Core.Infrastructure.DependencyManagement;

namespace Phatra.Core.Infrastructure
{
    public abstract class BaseEngine : IEngine
    {
        #region Fields

        private IContainerManager _containerManager;

        #endregion

        #region Utilities

        /// <summary>
        /// Run startup tasks
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(IEngineConfig config)
        {
            var builder = new ContainerBuilder();
            //var container = builder.Build();

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.


            //dependencies
            var typeFinder = CreateTypeFinder(config);
            builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<IEngineConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            //builder.RegisterModule<LoggingModule>();

            //builder.Update(container);

            //register dependencies provided by other assemblies
            //builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
            {
                dependencyRegistrar.Register(builder, typeFinder);
            }                
            //builder.Update(container);

            var container = builder.Build();

            _containerManager = CreateContainerManager(container);


            foreach (var dependencyRegistrar in drInstances)
            {
                dependencyRegistrar.SetDependencyResolver(container);
            }

            //set dependency resolver
            //SetDependencyResolver(container);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(IEngineConfig config)
        {
            //register dependencies
            RegisterDependencies(config);

            //startup tasks
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public IContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion

        #region Overideable Methods

        protected virtual ITypeFinder CreateTypeFinder(IEngineConfig config)
        {
            return new AppDomainTypeFinder();
        }

        protected virtual IContainerManager CreateContainerManager(IContainer container)
        {
            return new ContainerManager(container);
        }

        //protected abstract void SetDependencyResolver(IContainer container);

        #endregion
    }
}
