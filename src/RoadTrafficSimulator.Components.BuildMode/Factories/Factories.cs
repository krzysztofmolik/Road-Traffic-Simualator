using System;
using Common;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public class Factories
    {
        public Factories( IVertexContainerFactory vertexContainerFactory, IMouseHandlerFactory mouseHandlerFactory, IControlFactories controlFactory )
        {
            this.VertexContainerFactory = vertexContainerFactory.NotNull();
            this.MouseHandlerFactory = mouseHandlerFactory;
            this.ControlFactory = controlFactory;
        }

        public IVertexContainerFactory VertexContainerFactory { get; private set; }

        public IMouseHandlerFactory MouseHandlerFactory { get; set; }

        public IControlFactories ControlFactory { get; set; }
    }
}