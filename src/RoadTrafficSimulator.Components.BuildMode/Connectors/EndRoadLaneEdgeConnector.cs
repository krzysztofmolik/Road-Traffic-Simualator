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
            this.AddConnectEndWithHandler<RoadConnection>( edge =>
                                                               {
                                                                   this.CommonConnectEndWith( edge.LeftEdge );
                                                                   this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( edge.Parent, PriorityType.None ) );
                                                               } );
            this.AddConnectEndWithHandler<RoadJunctionEdge>( roadJunctionEdge =>
                                                                 {
                                                                     this.CommonConnectEndWith( roadJunctionEdge );
                                                                     this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( roadJunctionEdge.RoadJunctionParent, PriorityType.FromRight ) );
                                                                 } );
            this.AddConnectEndWithHandler<CarsRemover>( carsRemover =>
                                                            {
                                                                this.CommonConnectEndWith( carsRemover );
                                                                this._endRoadLaneEdge.Routes.AddRoute( new RouteElement( carsRemover, PriorityType.None ) );
                                                            } );

            this.AddConnectBeginWithHandler<CarsInserter>( carInserter => this.CommonConnectBeginWith( carInserter.RightEdge ) );
            this.AddConnectBeginWithHandler<RoadJunctionEdge>( this.CommonConnectBeginWith );
            this.AddConnectBeginWithHandler<RoadConnection>( edge => this.CommonConnectBeginWith( edge.RightEdge ) );


        }

        public ILogicControl PreviousEdge { get; private set; }
        public ILogicControl NextEdge { get; private set; }

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

        private void CommonConnectBeginWith( IEdge edge )
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