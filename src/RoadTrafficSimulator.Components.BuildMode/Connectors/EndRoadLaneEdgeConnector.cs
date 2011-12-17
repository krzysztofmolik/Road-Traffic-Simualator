using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class EndRoadLaneEdgeConnector : ConnectorBase
    {
        private readonly EndRoadLaneEdge _endRoadLaneEdge;

        public EndRoadLaneEdgeConnector( EndRoadLaneEdge endRoadLaneEdge )
        {
            this._endRoadLaneEdge = endRoadLaneEdge;
            this.AddStartFromHandler<RoadConnection>( edge =>
                                                               {
                                                                   this.CommonConnectEndWith( edge.Edge );
//                                                                   this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( edge, PriorityType.None ) );
                                                               } );
            this.AddStartFromHandler<JunctionEdge>( roadJunctionEdge =>
                                                                 {
                                                                     this.CommonConnectEndWith( roadJunctionEdge.InvertedEdge );
//                                                                     this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( roadJunctionEdge, PriorityType.None ) );
                                                                 } );
            this.AddStartFromHandler<CarsRemover>( carsRemover =>
                                                            {
                                                                this.CommonConnectEndWith( carsRemover.Edge );
//                                                                this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( carsRemover, PriorityType.None ) );
                                                            } );

            this.AddEndOnHandler<CarsInserter>( carInserter => this.CommonEndOnHandler( carInserter.RightEdge ) );
            this.AddEndOnHandler<JunctionEdge>( e => this.CommonEndOnHandler( e.InvertedEdge ) );
            this.AddEndOnHandler<RoadConnection>( edge => this.CommonEndOnHandler( edge.RightEdge ) );


        }

        public IEdge PreviousEdge { get; private set; }
        public IEdge NextEdge { get; private set; }

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

        private void CommonEndOnHandler( IEdge edge )
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

        private void CommonConnectEndWith( IEdge edge )
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