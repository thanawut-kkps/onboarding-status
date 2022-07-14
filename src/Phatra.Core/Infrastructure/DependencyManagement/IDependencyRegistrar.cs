using Autofac;

namespace Phatra.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        void SetDependencyResolver(IContainer container);

        int Order { get; }
    }
}
