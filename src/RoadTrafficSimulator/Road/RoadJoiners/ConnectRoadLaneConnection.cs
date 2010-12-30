using System;
using RoadTrafficSimulator.Infrastructure.Control;
using System.Linq;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class ConnectRoadLaneConnection : ConnectionCommandBase
    {
        public override bool CanConnect( IControl first, IControl second )
        {
            var roadLaneEdge = this.GetSpecifyType<EndRoadLaneEdge>( first, second );
            var roadConnection = this.GetSpecifyType<RoadLaneConnection>( first, second );
            return roadLaneEdge != null && roadConnection != null;
        }

        public override void Connect( IControl first, IControl second )
        {
            var roadLaneEdge = this.GetSpecifyType<EndRoadLaneEdge>( first, second );
            var roadConnection = this.GetSpecifyType<RoadLaneConnection>( first, second );
            if ( roadLaneEdge == null || roadConnection == null )
            {
                return;
            }

            if ( this.IsAnyRoadConnectedToConnection( roadConnection ) == false )
            {
                roadConnection.ConnectionSupport.Connect( roadLaneEdge.ConnectionSupport );
                roadLaneEdge.ConnectionSupport.Connect( roadConnection.ConnectionSupport );

                var endEdge = MyMathHelper.CreatePerpendicualrLine(
                                                roadLaneEdge.Location,
                                                roadConnection.Location,
                                                Constans.RoadHeight );

                roadConnection.Line.Begin = endEdge.Item1;
                roadConnection.Line.End = endEdge.Item2;
            }
        }

        private bool IsAnyRoadConnectedToConnection( RoadLaneConnection roadConnection )
        {
            return roadConnection.ConnectionSupport.ConnectedObject.Count() != 0;
        }
    }
}