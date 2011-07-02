using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class CreateControlCommand : IAction
    {
        private readonly Type _type;
        private readonly List<IParameter> _parameters = new List<IParameter>();
        private Guid _objectId;

        public static CreateControlCommand Create<T>( Guid id, Expression<Func<T>> constructor )
        {
            var command = new CreateControlCommand( id, typeof( T ) );
            var newExpression = constructor.Body as NewExpression;
            if ( newExpression == null ) { throw new ArgumentException( "constructor" ); }

            // TODO Doh... 
            foreach ( var expression in newExpression.Arguments )
            {
                if ( expression is MemberExpression )
                {
                    ProcessMemeberExpression<T>( command, ( MemberExpression ) expression );
                }
                else if ( expression is ConstantExpression )
                {
                    ProcessConstansExpression<T>( command, ( ConstantExpression ) expression );
                }
            }

            return command;
        }

        private static void ProcessConstansExpression<T>( CreateControlCommand command, ConstantExpression expression )
        {
            command._parameters.Add( new Parameter( expression.Value, expression.Type ) );
        }

        private static void ProcessMemeberExpression<T>( CreateControlCommand command, MemberExpression memberExpres )
        {
            var member2 = memberExpres.Expression as MemberExpression;
            var constExpres = member2.Expression as ConstantExpression;
            var control = ( ( FieldInfo ) member2.Member ).GetValue( constExpres.Value );
            if ( memberExpres.Member is PropertyInfo )
            {
                var fieldInfo = ( ( PropertyInfo ) memberExpres.Member );
                var value = fieldInfo.GetValue( control, null );
                command._parameters.Add( new Parameter( value, fieldInfo.PropertyType ) );
            }
            else
            {
                var fieldInfo = ( ( FieldInfo ) memberExpres.Member );
                var value = fieldInfo.GetValue( control );
                command._parameters.Add( new Parameter( value, fieldInfo.FieldType ) );
            }
        }

        public CreateControlCommand( Guid id, Type type )
        {
            this._objectId = id;
            this._type = type;
        }

        public void AddConstructParameters( IParameter parameter )
        {
            this._parameters.Add( parameter );
        }

        public void Execute( DeserializationContext context )
        {
            var types = this._parameters.Select( s => s.Type ).ToArray();
            var constructor = this._type.GetConstructor( types );
            Debug.Assert( constructor != null );

            var parameters = this._parameters.Select( s => s.GetValue( context ) ).ToArray();
            var obj = ( IControl ) constructor.Invoke( parameters );
            obj.Id = this._objectId;
            context.CreateControls.Add( obj );
        }

        public Order Priority { get { return Order.High; } }
    }
}