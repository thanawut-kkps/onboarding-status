using Autofac;
using Phatra.Core.Infrastructure.DependencyManagement;
using Phatra.Core.Logging;
using Phatra.Core.Managers;

namespace Phatra.Core.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 1;
            }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Logging 
            builder.RegisterModule<Log4NetModule>();
            builder.RegisterInstance(Log4NetLogger.CreateLoggerFor("Log4NetLogger")).As<ILogger>();
            builder.RegisterType<Log4NetLoggerFactory>().As<ILoggerFactory>();

            // Manager
            builder.RegisterType<WebCtrlManager>().As<IWebCtrlManager>().InstancePerLifetimeScope();
            builder.RegisterType<IfisManager>().As<IIfisManager>().InstancePerLifetimeScope();

        }

        public void SetDependencyResolver(IContainer container)
        {
            // There is no dependencyresolver here.
        }
    }
}
