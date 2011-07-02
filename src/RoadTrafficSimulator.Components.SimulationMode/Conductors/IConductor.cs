using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public interface IConductor
    {
        IRoadElement GetNextRandomElement( List<IRoadElement> route );
        void Take( Car car );
        bool ShouldChange( Vector2 acutalCarLocation, Car car );
        float GetDistanceToStopLine();
        void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation );
        void GetNextJunctionInformation( RouteMark route, JunctionInformation junctionInformation );
        void GetCarAheadDistance(IRouteMark routMark, CarInformation carInformation);
        Vector2 GetCarDirection( Car car );
        void Remove( Car car );
        float GetCarDistanceToEnd( Car car );
    }

    public class LightInfomration
    {
        public Car Car { get; set; }
        public float LightDistance { get; set; }
        public LightState LightState { get; set; }
    }

    public class JunctionInformation
    {
        public float JunctionDistance { get; set; }
        public Car Car { get; set; }
    }
}