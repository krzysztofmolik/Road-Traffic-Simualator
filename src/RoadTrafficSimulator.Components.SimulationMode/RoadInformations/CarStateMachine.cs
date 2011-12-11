using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class RoadInformation
    {
        public float Distance { get; set; }
    }

    public class RoadIterator
    {
        public void Process( Car car, IEngine engine )
        {
            var road = car.Conductors.Clone();
            var endDistance = road.Current.RoadInformation.GetCarDistanceTo( car, road.GetNext().RoadElement );
            var informatin = new RoadInformation
                                 {
                                     Distance = endDistance
                                 };

            var end = car.Conductors.Current.Process( car, informatin );
            if ( end ) { return; }

            while ( informatin.Distance < UnitConverter.FromMeter( 100.0f ) && road.MoveNext() )
            {
                var endProcessing = road.Current.Process( car, informatin );
                if ( endProcessing ) { break; }
                informatin.Distance += road.Current.RoadInformation.Lenght( road.GetPrevious().RoadElement, road.GetNext().RoadElement );
            }
        }
    }

    public class CarStateMachine
    {
        private readonly Car _car;
        private readonly IEngine _currentState;
        private readonly RoadIterator _roadIterator;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._currentState = new Engine();
            this._car = car;
            this._roadIterator = new RoadIterator();
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._car.Conductors.Current.RoadInformation.ShouldChange( this._car ) )
            {
                this.ChangeConductor();
            }

            this._roadIterator.Process( this._car, this._currentState );
        }

        private void ChangeConductor()
        {
            this._car.Conductors.Current.RoadInformation.OnExit( this._car );
            this._car.Conductors.MoveNext();
            this._car.Conductors.Current.RoadInformation.OnEnter( this._car );
            var direction = this._car.Conductors.Current.RoadInformation.GetCarDirection( this._car, this._car.Conductors.GetNext().RoadElement );
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