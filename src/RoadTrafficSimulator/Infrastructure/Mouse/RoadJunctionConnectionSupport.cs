using System;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class ConnectorBase
    {
        protected void ConnectBySubscribingToEvent( IControl firstPoint, IControl secondPoint )
        {
            firstPoint.Translated.Subscribe(s =>
                                                {
                                                    secondPoint.SetLocation( firstPoint.Location);
                                                    secondPoint.Redraw();
                                                });
        }
    }
}