using RoadTrafficSimulator.Components.BuildMode.Controls;
using System;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class JunctionEdgeConnector
    {
        private readonly JunctionEdge _owner;

        public JunctionEdgeConnector( JunctionEdge owner )
        {
            this._owner = owner;
        }

        public IEdge Edge { get; private set; }
        public InternalRoadJunctionEdge JunctionEdge { get; private set; }
        public LightBlock Light { get; private set; }

        public void ConnectEndsOn( RoadLaneBlock roadLaneEdge )
        {
            this.Edge = roadLaneEdge.RightEdge;
        }

        public void ConnectBeginFrom( RoadLaneBlock roadLaneEdge )
        {
            this.Edge = roadLaneEdge.LeftEdge;
        }

        public void ConnectBeginFrom( JunctionEdge roadLaneEdge )
        {
            this.Edge = roadLaneEdge.Edge;
            this.Edge.StartPoint.Translated.Subscribe( s => this._owner.Edge.EndPoint.SetLocation( s.Control.Location ) );
            this.Edge.EndPoint.Translated.Subscribe( s => this._owner.Edge.StartPoint.SetLocation( s.Control.Location ) );
        }

        public void ConnectEndsOn( JunctionEdge roadLaneEdge )
        {
            this.Edge = roadLaneEdge.Edge;
            this.Edge.StartPoint.Translated.Subscribe( s => this._owner.Edge.EndPoint.SetLocation( s.Control.Location ) );
            this.Edge.EndPoint.Translated.Subscribe( s => this._owner.Edge.StartPoint.SetLocation( s.Control.Location ) );
        }

        public void ConnectWithJunction( RoadJunctionBlock junction, int edge )
        {
            this.JunctionEdge = junction.JunctionEdges[ edge ];

            this.JunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.Edge.EndPoint.SetLocation( s.Control.Location ) );
            this.JunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.Edge.StartPoint.SetLocation( s.Control.Location ) );
        }


        public bool CanPutLights()
        {
            return this.Edge is EndRoadLaneEdge;
        }

        public void ConnectWithLight( LightBlock light )
        {
            this.Light = light;
        }
    }
}