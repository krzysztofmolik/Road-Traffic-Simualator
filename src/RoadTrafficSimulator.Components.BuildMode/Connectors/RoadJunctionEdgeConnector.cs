using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class RoadJunctionEdgeConnector
    {
        private readonly RoadJunctionEdge _owner;

        public RoadJunctionEdgeConnector( RoadJunctionEdge owner )
        {
            this._owner = owner;
        }

        public Edge Edge { get; private set; }

        public bool AreAllSlotOccupied
        {
            get { return this.Edge != null; }
        }

        public void ConnectBeginWith( RoadJunctionEdge roadJunctionEdge )
        {
            roadJunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.EndPoint.SetLocation( s.Control.Location ) );
            roadJunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.StartPoint.SetLocation( s.Control.Location ) );
        }

        public void ConnectEndWith( RoadJunctionEdge roadJunctionEdge )
        {
            roadJunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.EndPoint.SetLocation( s.Control.Location ) );
            roadJunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.StartPoint.SetLocation( s.Control.Location ) );

            var tranlationVector = roadJunctionEdge.Location - this._owner.Location;
            this._owner.Parent.Translate( tranlationVector.ToTranslationMatrix() );
        }

        public void ConnectBeginWith( Edge roadLaneEdge )
        {
            this._owner.StartPoint.Translated.Subscribe( s =>
                                                {
                                                    roadLaneEdge.EndPoint.SetLocation( ( ( IControl ) this._owner.StartPoint ).Location );
                                                    roadLaneEdge.EndPoint.Redraw();
                                                } );
            this._owner.EndPoint.Translated.Subscribe( s =>
                                                {
                                                    roadLaneEdge.StartPoint.SetLocation( ( ( IControl ) this._owner.EndPoint ).Location );
                                                    roadLaneEdge.StartPoint.Redraw();
                                                } );
            this.Edge = roadLaneEdge;
        }

        public void ConnectEndWith( Edge edge )
        {
            this.Edge = edge;
        }

        public bool AreConnected( RoadJunctionEdge edge )
        {
            return this.Edge == edge;
        }
    }
}