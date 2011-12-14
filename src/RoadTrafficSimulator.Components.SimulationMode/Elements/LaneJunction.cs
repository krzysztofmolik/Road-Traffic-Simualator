using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    // BUG
    public class LaneJunction : RoadElementBase
    {
        private readonly Func<LaneJunction, IRoadInformation> _condutorFactory;
        private Light.Light _lights;
        private readonly LaneJunctionDrawer _drawer;

        public LaneJunction( RoadJunctionBlock control, Func<LaneJunction, IRoadInformation> condutorFactory )
            : base( control )
        {
            this.JunctionBuilder = control;

            this._condutorFactory = condutorFactory;
            this.RoadInformation = this._condutorFactory( this );
            this._drawer = new LaneJunctionDrawer( this );
        }

        public IRoadInformation RoadInformation { get; private set; }
        public Light.Light Lights { get { return this._lights; } }
        public RoadJunctionBlock JunctionBuilder { get; private set; }

        public JunctionEdge Top { get; set; }
        public JunctionEdge Bottom { get; set; }
        public JunctionEdge Left { get; set; }
        public JunctionEdge Right { get; set; }

        public override IDrawer Drawer
        {
            get { return this._drawer; }
        }

        public void AddLight( Light.Light light )
        {
            this._lights = light;
            // BUG
            this.RoadInformation = this._condutorFactory( this );
        }
    }
}