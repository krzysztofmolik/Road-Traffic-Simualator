using System;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class ConnectRoadLaneWithRoadJunction : IConnectionCommand
    {
        public void Connect( IControl first, IControl second )
        {
            var roadLaneEdge = this.GetSpecifiyObject<EndRoadLaneEdge>( first, second );
            var roadJunctionEdge = this.GetSpecifiyObject<RoadJunctionEdge>( first, second );
            if ( roadLaneEdge == null || roadJunctionEdge == null )
            {
                return;
            }

            roadLaneEdge.StartPoint.Changed.Subscribe(
                s => roadJunctionEdge.EndPoint.SetLocation( roadLaneEdge.StartPoint.Location ) );
            roadLaneEdge.EndPoint.Changed.Subscribe( s => roadJunctionEdge.StartPoint.SetLocation( roadLaneEdge.EndPoint.Location ) );

            roadJunctionEdge.StartPoint.Changed.Subscribe(
                s => roadLaneEdge.EndPoint.SetLocation( roadJunctionEdge.StartPoint.Location ) );
            roadJunctionEdge.EndPoint.Changed.Subscribe( s => roadLaneEdge.StartPoint.SetLocation( roadJunctionEdge.EndPoint.Location ) );

            var roadLaneParent = roadJunctionEdge.Parents.OfType<IRoadLaneBlock>().FirstOrDefault();
            if ( roadLaneParent == null )
            {
                return;
            }
        }

        public bool CanConnect( IControl first, IControl second )
        {
            var roadLane = this.GetSpecifiyObject<EndRoadLaneEdge>( first, second );
            var roadJunction = this.GetSpecifiyObject<RoadJunctionEdge>( first, second );
            if ( roadLane == null || roadJunction == null )
            {
                return false;
            }
            return true;
        }

        private TTarget GetSpecifiyObject<TTarget>( IControl first, IControl second ) where TTarget : class
        {
            var firstRoadJunction = first as TTarget;
            if ( firstRoadJunction != null )
            {
                return firstRoadJunction;
            }

            return second as TTarget;
        }
    }
}