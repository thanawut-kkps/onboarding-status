using Autofac;
using OnboardingStatus_Web.ClientService;
using Phatra.Core.Infrastructure;
using Phatra.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnboardingStatus_Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 3;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ClientServiceClient>().As<IClientService>().InstancePerLifetimeScope();
        }

        public void SetDependencyResolver(IContainer container)
        {
            //
        }
    }
}