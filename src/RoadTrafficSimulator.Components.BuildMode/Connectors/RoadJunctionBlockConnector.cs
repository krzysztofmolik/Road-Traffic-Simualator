using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using System;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class RoadJunctionBlockConnector
    {
        private readonly JunctionEdge[] _edges = new JunctionEdge[ EdgeType.Count ];

        private readonly RoadJunctionBlock _owner;

        public RoadJunctionBlockConnector( RoadJunctionBlock owner )
        {
            this._owner = owner;
        }

        public JunctionEdge LeftEdge { get { return this._edges[ EdgeType.Left ]; } }
        public JunctionEdge RightEdge { get { return this._edges[ EdgeType.Right ]; } }
        public JunctionEdge TopEdge { get { return this._edges[ EdgeType.Top ]; } }
        public JunctionEdge BottomEdge { get { return this._edges[ EdgeType.Bottom ]; } }

        public JunctionEdge[] Edges { get { return this._edges; } }

        public void ConnectStartOn( JunctionEdge junctionEdge, int side )
        {
            this._edges[ side ] = junctionEdge;

            var edge = this._owner.JunctionEdges[ side ];
            junctionEdge.Edge.StartPoint.Translated.Subscribe( s => edge.EndPoint.SetLocation( s.Control.Location ) );
            junctionEdge.Edge.EndPoint.Translated.Subscribe( s => edge.StartPoint.SetLocation( s.Control.Location ) );
        }
    }
}