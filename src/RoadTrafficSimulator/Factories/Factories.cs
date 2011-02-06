using System;
using Common;

namespace RoadTrafficSimulator.Factories
{
    public class Factories
    {
        public Factories( IVertexContainerFactory vertexContainerFactory)
        {
            this.VertexContainerFactory = vertexContainerFactory.NotNull();
        }

        public IVertexContainerFactory VertexContainerFactory { get; private set; }
    }
}