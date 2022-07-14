using System;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Phatra.Core.Infrastructure.DependencyManagement;

namespace Phatra.Core.Web.Infrastructure.DependencyManagement
{
    public class WebContainerManager : ContainerManager
    {
        public WebContainerManager(IContainer container)
            : base(container)
        {
            //
        }


        public override ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                return base.Scope();
            }
            catch (Exception)
            {
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }       
        }
    }
}
