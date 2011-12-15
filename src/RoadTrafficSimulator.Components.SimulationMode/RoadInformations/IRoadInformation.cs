using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IRoadInformation
    {
        void OnEnter( Car car );
        void OnExit(Car car);
        float Lenght(IRoadElement previous, IRoadElement next);
        bool ShouldChange( Car car );
        CarAhedInformation GetCarAheadDistance( Car car );
        FirstCarToOutInformation GetFirstCarToOutInformation();
        Vector2 GetCarDirection( Car car, IRoadElement nextPoint );
        float GetCarDistanceTo( Car car, IRoadElement nextPoint );

        // Create builder?
        void SetConnection( IRoadElement roadElement );
        void SetReversConnection( IRoadElement roadElement );
        IEnumerable<IRoadElement> ReversConnection { get; }
    }
}
