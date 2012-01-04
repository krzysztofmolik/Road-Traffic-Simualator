using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class InternalRoadJunctionEdge : Edge, IComponent
    {
        private readonly InternalRoadJunctionEdgeVertexContainer _vertexContainer;
        private readonly RoadJunctionBlock _parent;

        public InternalRoadJunctionEdge( Factories.Factories factories, RoadJunctionBlock roadJunctionBlock, int index )
            : base( factories, Styles.NormalStyle, roadJunctionBlock )
        {
            this._vertexContainer = new InternalRoadJunctionEdgeVertexContainer( this );
            this._parent = roadJunctionBlock;
            this.EdgeIndex = index;
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }

        public int EdgeIndex { get; private set; }

        public override bool IsHitted( Microsoft.Xna.Framework.Vector2 location )
        {
            return false;
        }

        public override ILogicControl GetHittedControl( Microsoft.Xna.Framework.Vector2 point )
        {
            return null;
        }
    }
}