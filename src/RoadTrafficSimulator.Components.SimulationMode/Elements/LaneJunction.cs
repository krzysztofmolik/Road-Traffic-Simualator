using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class LaneJunction : RoadElementBase
    {
        public LaneJunction( RoadJunctionBlock control )
            : base( control )
        {
            this.JunctionBuilder = control;
            this.Left = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Left ] );
            this.Top = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Top ] );
            this.Right = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Right ] );
            this.Bottom = new JunctionEdge( control.RoadJunctionEdges[ EdgeType.Bottom ] );

            this.Left.Junction = this;
            this.Top.Junction = this;
            this.Right.Junction = this;
            this.Bottom.Junction = this;
        }

        public RoadJunctionBlock JunctionBuilder { get; private set; }
        public JunctionEdge Left { get; set; }
        public JunctionEdge Top { get; set; }
        public JunctionEdge Right { get; set; }
        public JunctionEdge Bottom { get; set; }

        public override IConductor Condutor
        {
            get { return null; }
        }
    }
}