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

        public ILogicControl PreviousEdge { get; private set; }
        public ILogicControl NextEdge { get; private set; }

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

        public void ConnectBeginWith( RoadConnection edge )
        {
            if ( this.PreviousEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.PreviousEdge = edge.RightEdge;
            edge.RightEdge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.RightEdge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            this.UpdateEndPointLocation(edge.RightEdge.StartPoint);
            this.UpdateStartPointLocation( edge.RightEdge.EndPoint );
        }

        public void ConnectEndWith( RoadConnection edge )
        {
            if ( this.NextEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.NextEdge = edge.LeftEdge;
            edge.LeftEdge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.LeftEdge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.RecalculatePosition() );
            this.UpdateStartPointLocation( edge.LeftEdge.EndPoint );
            this.UpdateEndPointLocation( edge.LeftEdge.StartPoint );
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
            this.UpdateStartPointLocation( edge.EndPoint );
            this.UpdateEndPointLocation( edge.StartPoint );
        }
    }
}