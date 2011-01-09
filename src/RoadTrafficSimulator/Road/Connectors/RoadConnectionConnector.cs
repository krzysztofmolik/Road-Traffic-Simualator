using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class RoadConnectionConnector : ConnectorBase
    {
        private const int MAX_CONNECTED_OBJECT = 2;

        private readonly RoadConnectionEdge _owner;

        public RoadConnectionConnector( RoadConnectionEdge owner )
            : base( MAX_CONNECTED_OBJECT )
        {
            this._owner = owner;
        }

        public void ConnectTo( EndRoadLaneEdge roadLaneEdge )
        {
            var otherSideOfLane = this.GetLaneEdgeOpositeTo( roadLaneEdge );

            var endEdge = MyMathHelper.CreatePerpendicualrLine(
                                            this._owner.Location,
                                            otherSideOfLane.Location,
                                            Constans.RoadHeight );

            this._owner.StartPoint.SetLocation( endEdge.Item2 );
            this._owner.EndPoint.SetLocation( endEdge.Item1 );

            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.StartPoint );
        }

        public void ConnectWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.StartPoint );
        }

        private EndRoadLaneEdge GetLaneEdgeOpositeTo( EndRoadLaneEdge roadLaneEdge )
        {
            var owner = roadLaneEdge.RoadLaneBlockParent;
            return owner.LeftEdge == roadLaneEdge ? owner.RightEdge : owner.LeftEdge;
        }
    }
}