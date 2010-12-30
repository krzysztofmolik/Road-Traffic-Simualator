using System;
using System.Diagnostics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using System.Linq;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class ConnectRoadLaneSideEdge : IConnectionCommand
    {
        public void Connect( IControl first, IControl second )
        {
            var firstSideEdge = first as SideRoadLaneEdge;
            var secondSideEdge = second as SideRoadLaneEdge;
            if ( firstSideEdge == null || secondSideEdge == null )
            {
                return;
            }

            this.SetConnection( firstSideEdge, secondSideEdge );
            this.SetConnection( secondSideEdge, firstSideEdge );
            firstSideEdge.LaneType = LaneType.DottedLine;
            secondSideEdge.LaneType = LaneType.HiddenLine;
        }

        public bool CanConnect( IControl first, IControl second )
        {
            var firstSideEdge = first as SideRoadLaneEdge;
            var secondSideEdge = second as SideRoadLaneEdge;
            if ( firstSideEdge == null || secondSideEdge == null )
            {
                return false;
            }

            if ( firstSideEdge.RoadLaneBlockParent == secondSideEdge.RoadLaneBlockParent )
            {
                return false;
            }

            if ( this.AreConnectedByParrent( firstSideEdge, secondSideEdge ) )
            {
                return false;
            }

            return true;
        }

        private bool AreConnectedByParrent( SideRoadLaneEdge firstSideEdge, SideRoadLaneEdge secondSideEdge )
        {
            var parrentConnectionSupport = firstSideEdge.RoadLaneBlockParent.ConnectionSupport;
            return parrentConnectionSupport.ConnectedObject.Any( s => s == secondSideEdge.ConnectionSupport );
        }

        private void SetConnection( SideRoadLaneEdge first, SideRoadLaneEdge second )
        {
            first.RoadLaneBlockParent.ConnectionSupport.ConnectChildren( first, second.ConnectionSupport );
        }
    }
}