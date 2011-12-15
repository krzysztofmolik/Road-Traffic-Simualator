using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class DriverBrain
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private float _destinationSpeed;
        private float _destinationPosition;
        private bool _canDriver;

        public DriverBrain( Car car, IEngine engine )
        {
            this._car = car;
            this._engine = engine;
        }

        public void Process( TimeSpan timeFrame )
        {
            this._canDriver = true;
            var road = this._car.Conductors.Clone();
            var end = this._car.Conductors.Current.Process( this._car, road );

            var distance = 0.0f;
            this.ProcessAnser( end, distance );

            distance += this.GetLenght( road );

            while ( this._canDriver && road.MoveNext() )
            {
                var processInformation = road.Current.Process( this._car, road );
                this.ProcessAnser( processInformation, distance );

                distance += this.GetLenght( road );
            }

            this._engine.SetStopPoint( this._destinationPosition, this._destinationSpeed );
            this._engine.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private float GetLenght( IRouteMark<IConductor> road )
        {
            Contract.Ensures( Contract.Result<float>() > 0 );
            var previous = road.GetPrevious();
            var next = road.GetNext();
            return road.Current.Information.Lenght(
                previous != null ? previous.RoadElement : null,
                next != null ? next.RoadElement : null );
        }

        private void ProcessAnser( RoadInformation end, float distance )
        {
            if ( end.CarAhead != null )
            {
                this._destinationPosition = Math.Max( 0, distance + end.CarAheadDistance - UnitConverter.FromMeter( 1 ) - end.CarAhead.Lenght );
                this._destinationSpeed = end.CarAhead.Velocity;
                this._canDriver = false;
                return;
            }

            this._destinationPosition = float.MaxValue;
            this._destinationSpeed = this._car.MaxSpeed;
            this._canDriver = true;
        }
    }
}