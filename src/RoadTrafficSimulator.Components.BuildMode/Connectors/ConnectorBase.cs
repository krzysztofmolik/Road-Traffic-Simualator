using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public abstract class ConnectorBase
    {
        private readonly IDictionary<Type, Action<IControl>> _endOnHandlers = new Dictionary<Type, Action<IControl>>();
        private readonly IDictionary<Type, Action<IControl>> _startFromHandlers = new Dictionary<Type, Action<IControl>>();

        public void ConnectEndOn( IControl control )
        {
            var handler = this.GetEndOnHandler( control );
            handler( control );
        }

        public void ConnectStartFrom( IControl control )
        {
            var handler = this.GetEndHandler( control );
            handler( control );
        }

        private Action<IControl> GetEndOnHandler( IControl control )
        {
            return this.GetHandlerFrom( this._endOnHandlers, control );
        }
        
        private Action<IControl> GetEndHandler( IControl control )
        {
            return this.GetHandlerFrom( this._startFromHandlers, control );
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

        protected void AddEndOnHandler<TControl>( Action<TControl> handler ) where TControl : class,IControl
        {
            this._endOnHandlers.Add( typeof( TControl ), control => this.ExecuteHandler( handler, control ) );
        }

        protected void AddStartFromHandler<TControl>( Action<TControl> handler ) where TControl : class,IControl
        {
            this._startFromHandlers.Add( typeof( TControl ), control => this.ExecuteHandler( handler, control ) );
        }

        private void ExecuteHandler<TControl>( Action<TControl> handler, IControl control ) where TControl : class, IControl
        {
            var arg = control as TControl;
            if ( arg == null ) { throw new ArgumentException(); }
            handler( arg );
        }
    }
}