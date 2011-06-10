using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class Light : IRoadElement
    {
        private readonly LightBlock _lightBlock;

        public Light( LightBlock lightBlock )
        {
            this._lightBlock = lightBlock;
        }

        public LightBlock LightBlock { get { return this._lightBlock; } }

        public void Draw( Graphic graphic, GameTime gameTime )
        {
            this.LightBlock.VertexContainer.Draw( graphic );
        }

        public IRoadElement Owner { get; set; }

        public IControl BuildControl
        {
            get { return this._lightBlock ; }
        }

        public IConductor Condutor
        {
            get { return null; }
        }
    }
}