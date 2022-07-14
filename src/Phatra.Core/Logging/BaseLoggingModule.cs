using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Module = Autofac.Module;

namespace Phatra.Core.Logging
{
    public abstract class BaseLoggingModule : Module
    {
        protected abstract ILogger CreateLoggerFor(Type type);

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var type = registration.Activator.LimitType;

            if (HasConstructorDependencyOnLogger(type))
                registration.Preparing += InjectLoggerViaConstructor;

            var properties = GetLoggerProperties(type).ToList();
            if (properties.Count > 0)
            {
                registration.Activated += (s, e) => {
                    var logger = CreateLoggerFor(type);
                    foreach (var prop in properties)
                    {
                        prop.SetValue(e.Instance, logger, BindingFlags.SetProperty | BindingFlags.Instance, null, null, null);
                    }
                };
            }
        }

        private IEnumerable<PropertyInfo> GetLoggerProperties(Type type)
        {
            return type.GetProperties().Where(property => property.CanWrite && property.PropertyType == typeof(ILogger));
        }

        private bool HasConstructorDependencyOnLogger(Type type)
        {
            return type.GetConstructors()
                       .Any(ctor => ctor.GetParameters()
                                        .Any(parameter => parameter.ParameterType == typeof(ILogger)));
        }

        private void InjectLoggerViaConstructor(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Concat(Enumerable.Repeat(
                new ResolvedParameter(
                     (parameter, context) => parameter.ParameterType == typeof(ILogger), 
                     (p, i) => CreateLoggerFor(p.Member.DeclaringType))
            , 1));
        }
    }

}
