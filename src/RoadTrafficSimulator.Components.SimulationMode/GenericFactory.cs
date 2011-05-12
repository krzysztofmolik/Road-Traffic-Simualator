using System;
using System.Collections.Generic;
using Common.Extensions;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    // Move it
    public class GenericFactory<TDestination>
    {
        public class Registration
        {
            private Type _orginalType;
            private readonly GenericFactory<TDestination> _factory;

            public Registration( Type orginalType, GenericFactory<TDestination> factory )
            {
                this._orginalType = orginalType;
                this._factory = factory;
            }

            public void Create<T>()
            {
                var ctor = typeof(T).GetConstructor<TDestination>();
                this._factory.Register(this._orginalType, ctor);
            }
        }

        private IDictionary<Type, Func<TDestination>> _factories = new Dictionary<Type, Func<TDestination>>();

        protected Registration ForType<TType>()
        {
            return new Registration( typeof( TType ), this );
        }

        private void Register( Type type, Func<TDestination> factory )
        {
            this._factories.Add( type, factory );
        }
    }
}