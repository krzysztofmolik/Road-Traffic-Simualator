using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class StandardDrawer : IDrawer
    {
        private readonly IRoadElement _owner;

        public StandardDrawer( IRoadElement owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
        }

        public virtual void Draw( Graphic graphic, GameTime gameTime )
        {
            this._owner.BuildControl.VertexContainer.Draw( graphic );
        }
    }
}