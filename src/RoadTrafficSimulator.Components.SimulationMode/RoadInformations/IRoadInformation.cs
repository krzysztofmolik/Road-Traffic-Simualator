using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IRoadInformation
    {
        IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng );
        void OnEnter( Car car );
        bool ShouldChange(Car car);
        Car GetCarAheadDistance();
        Car GetFirstCarToOutInformation( IRoadInformation roadInformation );
        Vector2 GetCarDirection( Car car );
        void OnExit( Car car );
        float GetCarDistanceToEnd( Car car );
        bool IsPosibleToDriveFrom( IRoadElement roadElement );
        bool IsPosibleToDriveTo( IRoadElement roadElement );
        bool IsCarPresent( Car car );
        float Lenght( IRoadElement previous, IRoadElement next );
        float GetCarDistanceToBegin(Car carAhead);
    }
}
