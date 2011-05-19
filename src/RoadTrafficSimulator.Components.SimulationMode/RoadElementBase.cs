using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public abstract class RoadElementBase : IRoadElement
    {
        private readonly IControl _control;

        public RoadElementBase( IControl control )
        {
            this._control = control;
        }

        public void Draw( Graphic graphic, GameTime gameTime )
        {
            this._control.VertexContainer.Draw( graphic );
        }
        public void Update( GameTime time )
        {
            // TODO Implement
        }

        public IControl BuildControl
        {
            get { return this._control; }
        }

        public abstract IConductor Condutor { get; }
    }
}