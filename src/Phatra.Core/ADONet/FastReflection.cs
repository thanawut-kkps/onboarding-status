using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Phatra.Core.AdoNet
{
    public class FastReflection
    {
        private FastReflection() { }
        
        private readonly ConcurrentDictionary<dynamic, object> _setters = new ConcurrentDictionary<dynamic, object>();
        private readonly ConcurrentDictionary<dynamic, object> _getters = new ConcurrentDictionary<dynamic, object>();

        public IReadOnlyDictionary<string, Action<T, object>> GetSettersForType<T>()
        {
            var setters = _setters.GetOrAdd(
                new { Type = typeof(T) },
                d => ((Type)d.Type).GetTypeInfo().GetProperties().ToDictionary(p => p.Name, GetSetDelegate<T>)
                );
            return (IReadOnlyDictionary<string, Action<T, object>>)setters;
        }

        public IReadOnlyDictionary<string, Func<T, object>> GetGettersForType<T>()
        {
            var setters = _getters.GetOrAdd(
                new { Type = typeof(T) },
                d => ((Type)d.Type).GetTypeInfo().GetProperties().ToDictionary(p => p.Name, GetGetDelegate<T>)
                );
            return (IReadOnlyDictionary<string, Func<T, object>>)setters;
        }

        public static FastReflection Instance = new FastReflection();
        private static TypeInfo TypeInfo = typeof(FastReflection).GetTypeInfo();

        static Action<T, object> GetSetDelegate<T>(PropertyInfo p)
        {
            var method = p.GetSetMethod();
            var genericHelper = TypeInfo.GetMethod(nameof(CreateSetterDelegateHelper), BindingFlags.Static | BindingFlags.NonPublic);
            var constructedHelper = genericHelper.MakeGenericMethod(typeof(T), method.GetParameters()[0].ParameterType);
            return (Action<T, object>)constructedHelper.Invoke(null, new object[] { method });
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        // ReSharper disable once UnusedMember.Local
        static object CreateSetterDelegateHelper<TTarget, TProperty>(MethodInfo method) where TTarget : class
        {
            var action = (Action<TTarget, TProperty>)method.CreateDelegate(typeof(Action<TTarget, TProperty>));
            Action<TTarget, object> ret = (target, param) => action(target, ConvertTo<TProperty>.From(param));
            return ret;
        }
       
        static Func<T, object> GetGetDelegate<T>(PropertyInfo p)
        {
            var method = p.GetGetMethod();
            var genericHelper = TypeInfo.GetMethod(nameof(CreateGetterDelegateHelper), BindingFlags.Static | BindingFlags.NonPublic);
            var constructedHelper = genericHelper.MakeGenericMethod(typeof(T), method.ReturnType);
            return (Func<T, object>)constructedHelper.Invoke(null, new object[] { method });
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        // ReSharper disable once UnusedMember.Local
        static object CreateGetterDelegateHelper<TTarget, TProperty>(MethodInfo method) where TTarget : class
        {
            var func = (Func<TTarget, TProperty>)method.CreateDelegate(typeof(Func<TTarget, TProperty>));
            Func<TTarget, object> ret = target => ConvertTo<TProperty>.From(func(target));
            return ret;
        }

    }
}
