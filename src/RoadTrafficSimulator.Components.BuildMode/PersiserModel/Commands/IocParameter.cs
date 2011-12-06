using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Autofac;
using System.Linq;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class IocParameter<T> : IAction
    {
        private readonly Type _type;
        private readonly Guid _id = Guid.NewGuid();

        public IocParameter()
        {
            this._type = typeof( T );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public object Execute( DeserializationContext context )
        {
            return context.IoC.Resolve( this._type );
        }

        public Type Type
        {
            get { return _type; }
        }

        public Guid CommandId
        {
            get { return this._id; }
        }
    }
}