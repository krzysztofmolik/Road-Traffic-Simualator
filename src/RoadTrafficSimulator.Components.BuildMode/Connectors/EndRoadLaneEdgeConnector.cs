﻿using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
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

        public void ConnectBeginWith( IEdge edge )
        {
            if ( this.PreviousEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.PreviousEdge = edge;
            edge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.Invalidate() );
            this.UpdateEndPointLocation( edge.StartPoint );
            this.UpdateStartPointLocation( edge.EndPoint );
        }

        private void UpdateEndPointLocation( IControl control )
        {
            this._endRoadLaneEdge.EndPoint.SetLocation( control.Location );
            this._endRoadLaneEdge.EndPoint.Redraw();
        }

        private void UpdateStartPointLocation( IControl control )
        {
            this._endRoadLaneEdge.StartPoint.SetLocation( control.Location );
            this._endRoadLaneEdge.StartPoint.Redraw();
        }

        // TODO This the same as overload function, remove this one
        public void ConnectBeginWith( RoadConnection edge )
        {
            if ( this.PreviousEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.PreviousEdge = edge.RightEdge;
            edge.RightEdge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.RightEdge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.Invalidate() );
            this.UpdateEndPointLocation( edge.RightEdge.StartPoint );
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
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.Invalidate() );
            this.UpdateStartPointLocation( edge.LeftEdge.EndPoint );
            this.UpdateEndPointLocation( edge.LeftEdge.StartPoint );
        }

        public void ConnectEndWith( IEdge edge )
        {
            if ( this.NextEdge != null )
            {
                throw new InvalidOperationException();
            }

            this.NextEdge = edge;
            edge.StartPoint.Translated.Subscribe( s => this.UpdateEndPointLocation( s.Control ) );
            edge.EndPoint.Translated.Subscribe( s => this.UpdateStartPointLocation( s.Control ) );
            edge.Translated.Subscribe( s => this._endRoadLaneEdge.Invalidate() );
            this.UpdateStartPointLocation( edge.EndPoint );
            this.UpdateEndPointLocation( edge.StartPoint );
        }
    }
}