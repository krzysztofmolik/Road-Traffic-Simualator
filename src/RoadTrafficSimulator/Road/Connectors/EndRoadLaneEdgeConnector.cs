using System;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class EndRoadLaneEdgeConnector
    {
        private readonly EndRoadLaneEdge _endRoadLaneEdge;

        public EndRoadLaneEdgeConnector( EndRoadLaneEdge endRoadLaneEdge )
        {
            this._endRoadLaneEdge = endRoadLaneEdge;
        }

        public Edge PreviousEdge { get; private set; }
        public Edge NextEdge { get; private set; }

        public void ConnectBeginWith( Edge edge )
        {
            if ( this.PreviousEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.PreviousEdge = edge;
            edge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            this.UpdateEndPointLocation( edge.StartPoint );
            this.UpdateStartPointLocation( edge.EndPoint );
        }

        private void UpdateEndPointLocation( IControl control )
        {
            this._endRoadLaneEdge.EndPoint.SetLocation( control.Location );
        }

        private void UpdateStartPointLocation( IControl control )
        {
            this._endRoadLaneEdge.StartPoint.SetLocation( control.Location );
        }

        public void ConnectEndWith( Edge edge )
        {
            if ( this.NextEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.NextEdge = edge;
            edge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.RecalculatePosition() );
            this.UpdateStartPointLocation(edge.EndPoint);
            this.UpdateEndPointLocation(edge.StartPoint);
        }
    }
}