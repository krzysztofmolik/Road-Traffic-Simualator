using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
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
            var endDistance = road.Current.GetDistanceToEnd( car );
            var informatin = new RoadInformation();
            informatin.Distance = endDistance;

            while ( informatin.Distance < UnitConverter.FromMeter( 100.0f ) && road.MoveNext() )
            {
                var endProcessing = road.Current.Process( car, informatin );
                if ( endProcessing ) { break; }
                informatin.Distance += road.Current.Length;
            }
        }
    }

    public class CarStateMachine
    {
        private readonly Car _car;
        private readonly IEngine _currentState;
        private readonly RoadIterator _roadIterator;
        private IConductor _currentConductor;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._currentState = new Engine();
            this._car = car;
            this._roadIterator = new RoadIterator();
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._currentConductor == null )
            {
                this._currentConductor = this._car.Conductors.Current;
            }

            if ( this._currentConductor.ShouldChange( this._car ) )
            {
                this._car.Conductors.MoveNext();
                this.ChangeConductor( this._car.Conductors.Current );
            }

            this._roadIterator.Process( this._car, this._currentState );
        }

        private void ChangeConductor( IConductor roadElement )
        {
            this._currentConductor.OnExit( this._car );
            this._currentConductor.OnEnter( this._car );
            var direction = this._currentConductor.GetCarDirection( this._car );
            if ( direction == Vector2.Zero )
            {
                this._car.Direction = direction;
            }
            else
            {
                this._car.Direction = Vector2.Normalize( this._currentConductor.GetCarDirection( this._car ) );
            }
        }
    }
}