using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public interface IControlers
    {
        void AddControl( IRoadElement element );
        void Draw( GameTime gameTime );
        void Update( GameTime gameTime );
        int Order { get; }
    }
}