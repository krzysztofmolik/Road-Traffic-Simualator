using System;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public interface IParameter
    {
        object GetValue( DeserializationContext context );
        Type Type { get; }
    }

    [Serializable]
    public class Parameter : IParameter
    {
        private readonly object _value;
        private readonly Type _type;

        public static Parameter Create<T>( T value )
        {
            return new Parameter( value, typeof( T ) );
        }

        public Parameter( object value, Type type )
        {
            this._value = value;
            this._type = type;
        }

        public object GetValue( DeserializationContext context )
        {
            return _value;
        }

        public Type Type
        {
            get { return _type; }
        }
    }
}