using RoadTrafficSimulator.Infrastructure.DependencyInjection;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public class Graphic
    {
        private readonly VertexPositionColorDrawer _vertexPositionColorDrawer;
        private readonly VertexPositionTextureDrawer _vertexPositionTextureDrawer;


        public Graphic( VertexPositionColorDrawer vertexPositionColorDrawer,
                        VertexPositionTextureDrawer vertexPositionTextureDrawer )
        {
            this._vertexPositionColorDrawer = vertexPositionColorDrawer;
            this._vertexPositionTextureDrawer = vertexPositionTextureDrawer;
        }

        public VertexPositionColorDrawer VertexPositionalColorDrawer
        {
            get { return this._vertexPositionColorDrawer; }
        }

        public VertexPositionTextureDrawer VertexPositionalTextureDrawer
        {
            get { return this._vertexPositionTextureDrawer; }
        }

    }
}