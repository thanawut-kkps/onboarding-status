using Autofac;
using Phatra.Core.Infrastructure;
using Phatra.Core.Configuration;
using Phatra.Core.Web.Infrastructure.DependencyManagement;
using Phatra.Core.Infrastructure.DependencyManagement;

namespace Phatra.Core.Web.Infrastructure
{
    public class WebEngine : BaseEngine
    {
        protected override ITypeFinder CreateTypeFinder(IEngineConfig config)
        {
            return new WebAppTypeFinder(config);
        }

        protected override IContainerManager CreateContainerManager(IContainer container)
        {
            return new WebContainerManager(container);
        }
    }
}
