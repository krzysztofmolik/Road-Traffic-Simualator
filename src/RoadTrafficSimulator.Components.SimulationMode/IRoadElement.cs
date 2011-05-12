using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IRoadElement
    {
        void Draw( Graphic graphic, GameTime gameTime );
        void Update( GameTime time );
    }
}