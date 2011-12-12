using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class LaneJucntionConductorRightHandJunctionInformation
    {
        private readonly LaneJunction _laneJunction;
        public LaneJucntionConductorRightHandJunctionInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }
        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.FirstOrDefault(s => s.ConnectedEdge == roadElement);
            return item;
        }

        public bool IsPosibleToDriverFrom( IRoadElement roadElement )
        {
            return this._laneJunction.Edges.Where( s => s.Situation.IsOut == false ).Any( s => s.ConnectedEdge == roadElement );
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._laneJunction.Edges.Where( s => s.Situation.IsOut ).Any( s => s.ConnectedEdge == roadElement );
        }

        public float Length( IRoadElement previous, IRoadElement next )
        {
            var previousEdge = this.GetEdgeConnectedWith( previous );
            var nextEdge = this.GetEdgeConnectedWith(next);

            return Vector2.Distance(previousEdge.EdgeBuilder.Location, nextEdge.EdgeBuilder.Location);
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            var edge = this.GetEdgeConnectedWith( nextPoint );
            return edge.EdgeBuilder.Location - car.Location;
        }
    }
}