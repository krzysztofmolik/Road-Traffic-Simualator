using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Factories
{
    public interface IVertexContainerFactory
    {
        IVertexContainer<VertexPositionColor> Create(RoadJunctionBlock roadJunctionBlock);
    }

    public class VertexContainerFactory : IVertexContainerFactory
    {
        public IVertexContainer<VertexPositionColor> Create( RoadJunctionBlock roadJunctionBlock )
        {
            return new RoadJunctionBlockVertexContainer( roadJunctionBlock );
        }
    }
}