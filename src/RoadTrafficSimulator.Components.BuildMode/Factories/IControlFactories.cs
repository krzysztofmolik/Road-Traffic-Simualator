using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public interface IControlFactories
    {
        RoadJunctionBlock CreateRoadJunctioBlockWithEdges( Vector2 location );
    }
}