using System;
using Autofac;

namespace Phatra.Core.Infrastructure.DependencyManagement
{
    public interface IContainerManager
    {
        T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class;

        object Resolve(Type type, ILifetimeScope scope = null);

        T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null);

        T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class;

        object ResolveUnregistered(Type type, ILifetimeScope scope = null);

        bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance);

        bool IsRegistered(Type serviceType, ILifetimeScope scope = null);

        object ResolveOptional(Type serviceType, ILifetimeScope scope = null);

        ILifetimeScope Scope();

    }
}
