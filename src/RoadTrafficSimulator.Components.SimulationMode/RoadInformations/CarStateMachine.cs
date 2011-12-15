using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
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
            if ( !this._car.Conductors.MoveNext() ) { return; }
            this._car.Conductors.Current.Information.OnEnter( this._car );

            if ( this._car.Conductors.GetNext() == null ) { return; }

            var direction = this._car.Conductors.Current.Information.GetCarDirection( this._car, this._car.Conductors.GetNext().RoadElement );
            Console.WriteLine( "{0} direction {1}", this._car.Conductors.Current.RoadElement.GetType().Name, direction );
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