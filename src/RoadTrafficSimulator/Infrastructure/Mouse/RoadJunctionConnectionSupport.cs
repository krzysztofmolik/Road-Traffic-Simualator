using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class ConnectorBase
    {
        protected void ConnectBySubscribingToEvent( IControl firstPoint, IControl secondPoint )
        {
            firstPoint.Translated.Subscribe( s => this.SetLocation( secondPoint, firstPoint.Location ) );
        }

        private void SetLocation( IControl control, Vector2 location )
        {
            if ( control.Location == location )
            {
                return;
            }

            var diff = location - control.Location;
            control.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }
    }
}