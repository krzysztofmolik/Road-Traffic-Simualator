using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IDrawer
    {
        void Draw( Graphic graphic, GameTime gameTime );
    }
}