using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class JunctionEdge : RoadElementBase
    {
        public JunctionEdge( RoadJunctionEdge edge )
            : base( edge )
        {
            this.EdgeBuilder = edge;
        }

        protected RoadJunctionEdge EdgeBuilder { get; set; }

        public LaneJunction Junction { get; set; }
        public Lane Lane { get; set; }

        public override IConductor Condutor
        {
            get { return null; }
        }
    }
}