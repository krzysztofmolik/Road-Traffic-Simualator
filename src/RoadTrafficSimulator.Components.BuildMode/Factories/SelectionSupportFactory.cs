using System;
using Autofac;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public interface IVertexContainerFactory
    {
        IVertexContainer<VertexPositionColor> Create( RoadJunctionBlock roadJunctionBlock );
    }

    public class VertexContainerFactory : IVertexContainerFactory
    {
        private IContainer _container;

        public VertexContainerFactory( IContainer container )
        {
            this._container = container;
        }

        public IVertexContainer<VertexPositionColor> Create( RoadJunctionBlock roadJunctionBlock )
        {
            var textureManager = this._container.Resolve<TextureManager>();
            return new RoadJunctionBlockVertexContainer( roadJunctionBlock, Styles.NormalStyle );
        }
    }
}