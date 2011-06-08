using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public interface IConductor
    {
        IRoadElement GetNextRandomElement();
        void Take( Car car );
        bool SholdChange( Vector2 acutalCarLocation, Vector2 direction );
        float GetDistanceToStopLine();
        LightInfomration GetLightInformation();
        JunctionInformation GetNextJunctionInformation();
        CarInformation GetCarAheadDistance();
        Vector2 GetCarDirection( Car car );
        void Remove( Car car );
    }

    public class LightInfomration
    {
        public float LightDistance { get; set; }
    }

    public class JunctionInformation
    {
        public float JunctionDistance { get; set; }
    }
}