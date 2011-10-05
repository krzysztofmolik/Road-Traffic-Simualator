using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public abstract class ConnectorBase
    {
        private readonly IDictionary<Type, Action<IControl>> _beginWithHandlers = new Dictionary<Type, Action<IControl>>();
        private readonly IDictionary<Type, Action<IControl>> _endWithHandlers = new Dictionary<Type, Action<IControl>>();

        public void ConnectBegintWith( IControl control )
        {
            var handler = this.GetBeginHandler( control );
            handler( control );
        }

        public void ConnectEndWith( IControl control )
        {
            var handler = this.GetEndHandler( control );
            handler( control );
        }

        private Action<IControl> GetBeginHandler( IControl control )
        {
            return this.GetHandlerFrom( this._beginWithHandlers, control );
        }
        
        private Action<IControl> GetEndHandler( IControl control )
        {
            return this.GetHandlerFrom( this._endWithHandlers, control );
        }

        private Action<IControl> GetHandlerFrom( IDictionary<Type, Action<IControl>> from, IControl control )
        {
            Action<IControl> handler;
            if ( !from.TryGetValue( control.GetType(), out handler ) )
            {
                throw new InvalidOperationException( "Handler not found" );
            }

            return handler;
        }

        protected void AddConnectBeginWithHandler<TControl>( Action<TControl> handler ) where TControl : class,IControl
        {
            this._beginWithHandlers.Add( typeof( TControl ), control => this.ExecuteHandler( handler, control ) );
        }

        protected void AddConnectEndWithHandler<TControl>( Action<TControl> handler ) where TControl : class,IControl
        {
            this._endWithHandlers.Add( typeof( TControl ), control => this.ExecuteHandler( handler, control ) );
        }

        private void ExecuteHandler<TControl>( Action<TControl> handler, IControl control ) where TControl : class, IControl
        {
            var arg = control as TControl;
            if ( arg == null ) { throw new ArgumentException(); }
            handler( arg );
        }
    }
}