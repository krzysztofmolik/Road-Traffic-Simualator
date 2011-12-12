using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class DriverBrain
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private bool _canDriver;
        private float _destinationSpeed;
        private float _destinationPosition;

        public DriverBrain( Car car, IEngine engine )
        {
            this._car = car;
            this._engine = engine;
        }

        public void Process( TimeSpan timeFrame )
        {
            var road = this._car.Conductors.Clone();
            var end = this._car.Conductors.Current.Process( this._car, road );
            this.ProcessAnser( end );

            while ( this._canDriver && road.MoveNext() )
            {
                road.Current.Process( this._car, road );
            }

            this._engine.SetStopPoint( this._destinationPosition, this._destinationSpeed );
            this._engine.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private void ProcessAnser( RoadInformation end )
        {
            this._canDriver = true;
            this._destinationPosition = float.MaxValue;
            this._destinationSpeed = this._car.MaxSpeed;
        }
    }

    public class CarStateMachine
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private readonly DriverBrain _driverBrain;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._engine = new Engine();
            this._car = car;
            this._driverBrain = new DriverBrain( this._car, this._engine );
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._car.Conductors.Current.Information.ShouldChange( this._car ) )
            {
                this.ChangeConductor();
            }

            this._driverBrain.Process( timeFrame );
        }

        private void ChangeConductor()
        {
            this._car.Conductors.Current.Information.OnExit( this._car );
            if( !this._car.Conductors.MoveNext() ) { return; }
            this._car.Conductors.Current.Information.OnEnter( this._car );

            if( this._car.Conductors.GetNext()== null ) { return; }

            var direction = this._car.Conductors.Current.Information.GetCarDirection( this._car, this._car.Conductors.GetNext().RoadElement );
            if ( direction == Vector2.Zero )
            {
                this._car.Direction = direction;
            }
            else
            {
                this._car.Direction = Vector2.Normalize( direction );
            }
        }
    }
}