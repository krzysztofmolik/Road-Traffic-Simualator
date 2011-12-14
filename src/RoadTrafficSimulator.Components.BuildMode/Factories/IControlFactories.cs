using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Components.BuildMode.Factories
{
    public interface IControlFactories
    {
        void CreateRoadJunctioBlockWithEdges( Vector2 location );
    }
}