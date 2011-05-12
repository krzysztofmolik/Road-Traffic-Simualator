using RoadTrafficSimulator.Infrastructure.DependencyInjection;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public class Graphic
    {
        private readonly VertexPositionColorDrawer _vertexPositionColorDrawer;
        private readonly VertexPositionTextureDrawer _vertexPositionTextureDrawer;

        private readonly IContentManager _contentManger;

        public Graphic( VertexPositionColorDrawer vertexPositionColorDrawer,
                        VertexPositionTextureDrawer vertexPositionTextureDrawer,
            IContentManager contentManger)
        {
            this._vertexPositionColorDrawer = vertexPositionColorDrawer;
            this._vertexPositionTextureDrawer = vertexPositionTextureDrawer;
            this._contentManger = contentManger;
        }

        public VertexPositionColorDrawer VertexPositionalColorDrawer
        {
            get { return this._vertexPositionColorDrawer; }
        }

        public VertexPositionTextureDrawer VertexPositionalTextureDrawer
        {
            get { return this._vertexPositionTextureDrawer; }
        }

        public IContentManager ContentManager
        {
            get { return this._contentManger; }
        }
    }
}