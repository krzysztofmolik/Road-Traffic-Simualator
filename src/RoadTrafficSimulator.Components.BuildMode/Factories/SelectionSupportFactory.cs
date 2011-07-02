using System;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public interface IVertexContainerFactory
    {
        IVertexContainer<VertexPositionColor> Create(RoadJunctionBlock roadJunctionBlock);
    }

    [Serializable]
    public class VertexContainerFactory : IVertexContainerFactory
    {
        public IVertexContainer<VertexPositionColor> Create( RoadJunctionBlock roadJunctionBlock )
        {
            return new RoadJunctionBlockVertexContainer( roadJunctionBlock, Styles.NormalStyle );
        }
    }
}