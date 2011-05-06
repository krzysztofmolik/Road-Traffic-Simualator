using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Common.Handler
{
    public class CompositeHandler
    {
        private IDictionary<Type, IHandler> _handlers = new Dictionary<Type, IHandler>();

        public void Register<T>( Action<T> handler )
        {
            var hand = new Handler<T>( handler );
            this._handlers.Add( typeof( T ), hand );
        }

        public void Handle( object @object )
        {
            Contract.Requires( @object != null );
            var type = @object.GetType();
            this._handlers[ type ].Execute( @object );
        }

    }

    public class Handler<T> : IHandler
    {
        private readonly Action<T> _action;

        public Handler( Action<T> action )
        {
            this._action = action;
        }

        public void Execute( object @object )
        {
            var value = (T) @object;
            this._action(value);
        }
    }

    public interface IHandler
    {
        void Execute( object @object );
    }
}