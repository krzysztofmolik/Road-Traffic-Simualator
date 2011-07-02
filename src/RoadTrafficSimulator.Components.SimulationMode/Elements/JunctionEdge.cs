using RoadTrafficSimulator.Components.BuildMode.Controls;

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
        public Lane Lane { get; set; }
        public bool IsOut { get; set; }
        public IDrawer Drawer { get; private set; }

        public JunctionEdgeConductor Situation { get; private set; }
    }

    public class JunctionEdgeConductor
    {
        private readonly JunctionEdge _owner;

        public JunctionEdgeConductor( JunctionEdge owner )
        {
            this._owner = owner;
            TODO Fix
        }

        public JunctionEdge OnTheLeft { get; private set; }
        public JunctionEdge OnTheFront { get; private set; }
        public JunctionEdge OnTheRight { get; private set; }

        public void GetCarInformation()
        {
            if ( this._owner.IsOut == false )
            {
                return;
            }

            var carToOut = this._owner.Lane.Condutor.GetFirstCarToOut();
            if( carToOut == null ) { return; }
        }
    }
}