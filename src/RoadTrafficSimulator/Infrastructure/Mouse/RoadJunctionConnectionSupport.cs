using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class ConnectorBase : IConnector
    {
        private readonly List<IControl> _connectedObject;
        private readonly int _maxConnectedObject;

        protected ConnectorBase( int maxConnectedObject )
        {
            this._maxConnectedObject = maxConnectedObject;
            this._connectedObject = new List<IControl>( this._maxConnectedObject );
        }

        public int ConnectedObject
        {
            get { return this._connectedObject.Count; }
        }

        protected void AddConnectedObject( IControl connector )
        {
            if ( this._connectedObject.Count + 1 > this._maxConnectedObject )
            {
                throw new InvalidOperationException( "Free slot is not available" );
            }

            this._connectedObject.Add( connector );
        }

        protected void RemoveConnectedObject( IControl connector )
        {
            this._connectedObject.Remove( connector );
        }

        protected void ConnectBySubscribingToEvent( IControl firstPoint, IControl secondPoint )
        {
            firstPoint.Changed.Subscribe( s => this.SetLocation( secondPoint, firstPoint.Location ) );
            firstPoint.Invalidate();
        }

        private void SetLocation( IControl control, Vector2 location )
        {
            if ( control.Location == location )
            {
                return;
            }

            var diff = location - control.Location;
            control.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }
    }
}