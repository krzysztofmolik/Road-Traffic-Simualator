using RoadTrafficSimulator.Components.BuildMode.Controls;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using Routes = RoadTrafficSimulator.Components.SimulationMode.Builder.Routes;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class JunctionEdge
    {
        public JunctionEdge( RoadJunctionEdge edge )
        {
            this.EdgeBuilder = edge;
            this.Drawer = new JunctionEdgeDrawer( this );
            this.Situation = new JunctionEdgeConductor( this );
        }

        public RoadJunctionEdge EdgeBuilder { get; set; }
        public LaneJunction Junction { get; set; }
        public IRoadElement ConnectedEdge { get; set; }
        public IDrawer Drawer { get; private set; }
        public Routes Routes { get; set; }

        public JunctionEdgeConductor Situation { get; private set; }
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
            var edges = this._owner.Junction.Edges.ToArray();
            var ownerIndex = edges.Select( ( e, i ) => new { Index = i, Edge = e } ).Where( s => s.Edge == this._owner ).Select( s => s.Index ).First();
            this.OnTheRight = edges[ ( ( ownerIndex + 3 ) % 4 ) ];
            this.OnTheFront = edges[ ( ( ownerIndex + 2 ) % 4 ) ];
            this.OnTheLeft = edges[ ( ( ownerIndex + 1 ) % 4 ) ];
            if( this._owner.ConnectedEdge != null )
            {
//                this.IsOut = this._owner.EdgeBuilder.IsOut;
            }
        }

        public bool IsOut { get; private set; }
        public JunctionEdge OnTheLeft { get; private set; }
        public JunctionEdge OnTheFront { get; private set; }
        public JunctionEdge OnTheRight { get; private set; }
    }
}