using Microsoft.Xna.Framework;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.RoadComponents
{
    public interface IRoadElement
    {
        void Draw( Graphic graphic, GameTime gameTime );
        void Update( GameTime time );
    }
}