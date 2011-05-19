using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IRoadElement
    {
        void Draw( Graphic graphic, GameTime gameTime );
        void Update( GameTime time );
        // TODO Change name
        IControl BuildControl { get; }
        IConductor Condutor { get; }
    }
}