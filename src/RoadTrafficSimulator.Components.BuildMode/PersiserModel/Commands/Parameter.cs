using System;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public static class ParamterDeclaration
    {
        public static IocParamterDeclaration<T> Ioc<T>()
        {
            return new IocParamterDeclaration<T>();
        }
    }

    public class IocParamterDeclaration<T>
    {
        public IocParameter<T> ToParamter()
        {
            return null;
        }

        public static implicit operator T( IocParamterDeclaration<T> arg )
        {
            return default( T );
        }
        public static implicit operator int( IocParamterDeclaration<T> arg )
        {
            return 3;
        }
    }

    [Serializable]
    public class Parameter<T> : IAction
    {
        private readonly object _value;
        private readonly Type _type;
        private readonly Guid _commandId = Guid.NewGuid();

        public Parameter( T value)
        {
            this._value = value;
            this._type = typeof(T);
        }

        public object Execute( DeserializationContext context )
        {
            return _value;
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }
    }
}