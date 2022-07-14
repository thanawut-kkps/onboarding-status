using Phatra.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phatra.Core.Infrastructure;
using Autofac;

namespace OnboardingStatus.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 3;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //throw new NotImplementedException();
        }

        public void SetDependencyResolver(IContainer container)
        {
            //
        }
    }
}