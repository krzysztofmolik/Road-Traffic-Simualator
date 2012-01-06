using System;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class JunctionEdge : IRoadElement
    {
        private readonly Func<JunctionEdge, IRoadInformation> _roadInformationFactory;
        private readonly IRoadInformation _roadInformation;

        public JunctionEdge( BuildMode.Controls.JunctionEdge edge, Func<JunctionEdge, IRoadInformation> roadInformationFactory )
        {
            this._roadInformationFactory = roadInformationFactory;
            this._roadInformation = this._roadInformationFactory( this );
            this.EdgeBuilder = edge;
            this.Drawer = new JunctionEdgeDrawer( this );
        }

        public BuildMode.Controls.JunctionEdge EdgeBuilder { get; private set; }
        public LaneJunction Junction { get; set; }
        public IRoadElement Next { get; set; }

        public IControl BuildControl
        {
            get { return this.EdgeBuilder; }
        }

        public IDrawer Drawer { get; private set; }
        public IRoutes Routes { get; set; }

        public IRoadInformation Information
        {
            get { return this._roadInformation; }
        }
        public Light.Light Light { get; set; }
    }
}