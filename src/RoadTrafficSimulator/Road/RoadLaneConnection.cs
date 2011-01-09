using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulator.Road
{
    public class RoadConnectionEdge : Edge
    {
        private readonly RoadConnectionConnector _connector;

        public RoadConnectionEdge( Vector2 location, IControl parent )
            : base( parent )
        {
            this._connector = new RoadConnectionConnector(this);
            this.StartPoint.SetLocation( location + new Vector2( 0, Constans.RoadHeight / 2 ) );
            this.EndPoint.SetLocation( location - new Vector2( 0, Constans.RoadHeight / 2 ) );
        }

        public RoadConnectionConnector Connector
        {
            get { return this._connector; }
        }

    }
}