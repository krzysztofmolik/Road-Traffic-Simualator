using System;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class JunctionEdge : IRoadElement
    {
        private readonly Func<JunctionEdge, IRoadInformation> _roadInformationFactory;

        public JunctionEdge( BuildMode.Controls.JunctionEdge edge, Func<JunctionEdge, IRoadInformation> roadInformationFactory )
        {
            this._roadInformationFactory = roadInformationFactory;
            this.RoadInformation = this._roadInformationFactory( this );
            this.EdgeBuilder = edge;
            this.Drawer = new JunctionEdgeDrawer( this );
            this.Situation = new JunctionEdgeConductor( this );
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
        public JunctionEdgeConductor Situation { get; private set; }
        public IRoadInformation RoadInformation { get; private set; }
    }

    public class JunctionEdgeConductor
    {
        private readonly JunctionEdge _owner;

        public JunctionEdgeConductor( JunctionEdge owner )
        {
            this._owner = owner;
        }

        public void SetUp()
        {
            // BUG
            //            var edges = this._owner.Prev.Edges.ToArray();
            //            var ownerIndex = edges.Select( ( e, i ) => new { Index = i, Edge = e } ).Where( s => s.Edge == this._owner ).Select( s => s.Index ).First();
            //            this.OnTheRight = edges[ ( ( ownerIndex + 3 ) % 4 ) ];
            //            this.OnTheFront = edges[ ( ( ownerIndex + 2 ) % 4 ) ];
            //            this.OnTheLeft = edges[ ( ( ownerIndex + 1 ) % 4 ) ];
            //            if( this._owner.Next != null )
            //            {
            ////                this.IsOut = this._owner.EdgeBuilder.IsOut;
            //            }
        }

        public bool IsOut { get; private set; }
        public JunctionEdge OnTheLeft { get; private set; }
        public JunctionEdge OnTheFront { get; private set; }
        public JunctionEdge OnTheRight { get; private set; }
    }
}