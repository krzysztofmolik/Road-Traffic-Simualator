using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightConnector
    {
        private readonly LightBlock _owner;

        public LightConnector( LightBlock owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
        }

        public JunctionEdge Owner { get; private set; }

        public void ConnectWith( JunctionEdge edge )
        {
            this.Owner = edge;
            edge.Edge.StartPoint.Translated.Subscribe( e => this._owner.SetLocation( edge.Location ) );
            edge.Edge.EndPoint.Translated.Subscribe( e => this._owner.SetLocation( edge.Location ) );
        }
    }
}