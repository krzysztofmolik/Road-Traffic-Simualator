using Common;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public class Factories
    {
        public Factories( IVertexContainerFactory vertexContainerFactory, IMouseHandlerFactory mouseHandlerFactory)
        {
            this.VertexContainerFactory = vertexContainerFactory.NotNull();
            this.MouseHandlerFactory = mouseHandlerFactory;
        }

        public IVertexContainerFactory VertexContainerFactory { get; private set; }

        public IMouseHandlerFactory MouseHandlerFactory { get; set; }
    }
}