using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class LaneJunction : RoadElementBase
    {
        private readonly Func<LaneJunction, IConductor> _condutorFactory;
        private readonly Light.Light[] _lights = new Light.Light[ EdgeType.Count ];
        private IConductor _conductor;

        public LaneJunction( RoadJunctionBlock control, Func<LaneJunction, IConductor> condutorFactory )
            : base( control )
        {
            this.JunctionBuilder = control;

            this._condutorFactory = condutorFactory;
            this._conductor = this._condutorFactory( this );

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

        public IEnumerable<JunctionEdge> Edges
        {
            get
            {
                yield return this.Left;
                yield return this.Top;
                yield return this.Right;
                yield return this.Bottom;
            }
        }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }

        public void AddLight( int edge, Light.Light light )
        {
            Contract.Requires( edge > 0 && edge < EdgeType.Count );
            this._lights[ edge ] = light;
            this._conductor = this._condutorFactory( this );
        }
    }
}