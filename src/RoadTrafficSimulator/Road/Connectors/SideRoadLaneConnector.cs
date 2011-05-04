using System;
using RoadTrafficSimulator.Extension;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class SideRoadLaneConnector
    {
        private readonly SideRoadLaneEdge _owner;

        public SideRoadLaneConnector( SideRoadLaneEdge owner )
        {
            this._owner = owner;
        }

        public SideRoadLaneEdge SideRoadLaneEdge { get; private set; }

        // TODO Change name
        public void ConnectChangeName( SideRoadLaneEdge edge )
        {
            ((IControl) this._owner).Translated.Subscribe(s =>
                                                              {
                                                                  edge.SetLocation( ((IControl) this._owner).Location);
                                                                  ((IControl) edge).Redraw();
                                                              });
            this.SideRoadLaneEdge = edge;
        }
    }
}