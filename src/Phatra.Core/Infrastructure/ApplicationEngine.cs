using Autofac;
using Phatra.Core.Configuration;
using Phatra.Core.Infrastructure.DependencyManagement;

namespace Phatra.Core.Infrastructure
{
    public class ApplicationEngine : BaseEngine
    {
        protected override ITypeFinder CreateTypeFinder(IEngineConfig config)
        {
            return new AppDomainTypeFinder();
        }

        protected override IContainerManager CreateContainerManager(IContainer container)
        {
            return new ContainerManager(container);
        }
    }

}
