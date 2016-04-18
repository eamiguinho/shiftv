using System;
using System.Collections.Generic;

namespace Shiftv.Global
{
    public abstract class IocLight : IIoc
    {
        private Dictionary<Type, Type> _types;
        private Dictionary<Type, object> _singletons;
        private Dictionary<Type, Type> _singletonsT;

        protected IocLight()
        {
            Clear();
            InitializeContainer();
        }

        protected abstract void InitializeContainer();

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private readonly object _lock = new object();

        public object Resolve(Type type)
        {
            lock (_lock)
            {
                if (_types.ContainsKey(type)) return Activator.CreateInstance(_types[type]);
                if (_singletons.ContainsKey(type)) return _singletons[type];
                if (_singletonsT.ContainsKey(type))
                {
                    var x = Activator.CreateInstance(_singletonsT[type]);
                    _singletons.Add(type, x);
                    return x;
                }
                throw new Exception("Type not registed: " + type.Name);
            }
        }

        public void RegisterType<TInterface, TClass>()
            where TClass : TInterface
        {
            _types.Add(typeof(TInterface), typeof(TClass));
        }

        public void RegisterSingleton<TInterface, TClass>()
            where TClass : TInterface, new()
        {
            try
            {
                _singletonsT.Add(typeof(TInterface), typeof(TClass));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Clear()
        {
            _types = new Dictionary<Type, Type>();
            _singletons = new Dictionary<Type, object>();
            _singletonsT = new Dictionary<Type, Type>();
        }
    }
}