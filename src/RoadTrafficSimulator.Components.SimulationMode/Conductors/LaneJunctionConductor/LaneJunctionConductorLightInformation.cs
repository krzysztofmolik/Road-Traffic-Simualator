using System;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
{
    public class LaneJunctionConductorLightInformation
    {
        private readonly LaneJunction _laneJunction;

        public LaneJunctionConductorLightInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation )
        {
            if ( lightInformation.LightDistance > Constans.ToVirtualUnit( 70.0f ) ) { return; }

            var previousEdge = this.GetEdgeConnectedWith( routeMark.GetPrevious() );
            if ( this._laneJunction.Lights[ previousEdge.Item1 ] != null  == false )
            {
                lightInformation.LightState = this._laneJunction.Lights[ previousEdge.Item1 ].LightState;
            }
            else
            {
                var nextEdge = this.GetEdgeConnectedWith( routeMark.GetNext() );
                lightInformation.LightDistance += Vector2.Distance( previousEdge.Item2.EdgeBuilder.Location, nextEdge.Item2.EdgeBuilder.Location );
                routeMark.MoveNext();
                routeMark.Current.Condutor.GetLightInformation( routeMark, lightInformation );
            }
        }

        private Tuple<int,JunctionEdge> GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.Select( (e,i ) => new { Index = i, Edge = e} ).Where( s => s.Edge.Lane == roadElement ).FirstOrDefault();
            if( item == null ) { return null; }
            return Tuple.Create( item.Index, item.Edge );
        }
    }
}