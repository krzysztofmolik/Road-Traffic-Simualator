using System;
using System.Collections.Generic;
using System.ComponentModel;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public interface IAction
    {
        object Execute( DeserializationContext context );
        Order Priority { get; }
        Type Type { get; }
        Guid CommandId { get; }
    }

    [Serializable]
    public class ActionCollection : IAction
    {
        private readonly Order _order;
        private readonly List<IAction> _actions;
        private readonly Guid _commandId = Guid.NewGuid();
        private IAction _resultAction;

        public ActionCollection( Order order )
        {
            this._order = order;
            this._actions = new List<IAction>();
        }

        public ActionCollection Add( IAction action )
        {
            this._actions.Add( action );
            this._resultAction = action;
            return this;
        }

        public ActionCollection AddRange( IEnumerable<IAction> action )
        {
            if ( action == null ) throw new ArgumentNullException( "action" );
            var actionsArray = action.ToArray();
            this._actions.AddRange( actionsArray );
            var lastAction = actionsArray.LastOrDefault();
            if ( lastAction != null ) { this._resultAction = lastAction; }
            return this;
        }

        public object Execute( DeserializationContext context )
        {
            var result = default( object );
            foreach ( var action in _actions )
            {
                if ( this._resultAction == action )
                {
                    result = action.Execute( context );
                }
                else
                {
                    action.Execute( context );
                }
            }

            return result;
        }

        public Order Priority
        {
            get { return this._order; }
        }

        public Type Type
        {
            get { return this._resultAction != null ? this._resultAction.Type : typeof( void ); }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }

        public IAction Return( IAction resultAction )
        {
            if ( !this._actions.Contains( resultAction ) ) { throw new InvalidOperationException(); }
            this._resultAction = resultAction;
            return this;
        }
    }


    public static class Find
    {
        public static FindBuilder<T> In<T>( T @object ) where T : IControl
        {
            return new FindBuilder<T>( @object );
        }
    }

    public class FindBuilder<T> where T : IControl
    {
        private readonly T _instance;

        public FindBuilder( T instance )
        {
            this._instance = instance;
        }

        public TProperty Property<TProperty>( TProperty value )
        {
            Is.Context.Add( ControlProperties.Create( this._instance, value ) );
            return default( TProperty );
        }
    }

    public static class Is
    {
        public static readonly List<IAction> Context = new List<IAction>();

        public static T Ioc<T>()
        {
            Context.Add( new IocParameter<T>() );
            return default( T );
        }

        public static T Const<T>( T value )
        {
            Context.Add( new Parameter<T>( value ) );
            return default( T );
        }

        public static T Control<T>( T control ) where T : IControl
        {
            Context.Add( new ResolveBaseOnId<T>( control.Id ) );
            return default( T );
        }

        public static T Action<T>( IAction action )
        {
            if ( action.Type != typeof( T ) ) { throw new ArgumentException(); }
            Context.Add( action );
            return default( T );
        }
    }
}