using System;
using System.Linq;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class CallAction : IAction
    {
        private readonly MemberInfo[] _propertiesPath;
        private readonly Guid _ownerId;
        private readonly IAction[] _parameters;
        private readonly MethodInfo _methodInfo;
        private readonly Guid _commandId = Guid.NewGuid();


        public CallAction( Guid ownerId, MemberInfo[] propertiesPath, MethodInfo methodInfo, IAction[] parameters )
        {
            this._propertiesPath = propertiesPath;
            this._ownerId = ownerId;
            this._parameters = parameters;
            this._methodInfo = methodInfo;
        }

        public object Execute( DeserializationContext context )
        {
            object owner = context.GetById( this._ownerId );
            foreach ( var memberInfo in this._propertiesPath.Reverse() )
            {
                if ( memberInfo is PropertyInfo )
                {
                    owner = ( ( PropertyInfo ) memberInfo ).GetValue( owner, null );
                }
                else if ( memberInfo is FieldInfo )
                {
                    owner = ( ( FieldInfo ) memberInfo ).GetValue( owner );
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            var parameters = this._parameters.Select( s => s.Execute( context ) ).ToArray();
            return this._methodInfo.Invoke( owner, parameters );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public Type Type
        {
            get { return this._methodInfo.ReturnType; }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }
    }
}