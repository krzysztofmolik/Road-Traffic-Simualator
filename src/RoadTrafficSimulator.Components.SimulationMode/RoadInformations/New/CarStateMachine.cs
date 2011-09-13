using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New
{
    public class CarStateMachine
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private readonly CarAheadIterator _carAheadIterator;

        public CarStateMachine( Car car, IEngine engine )
        {
            this._car = car;
            this._engine = engine;
            this._carAheadIterator = new CarAheadIterator();
        }

        public void Update( GameTime time )
        {
            if ( this._car.Conductors.Current.ShouldChange( this._car) ) { this.SwitchToNextConductor(); }
            if ( this._car.RoadElements.Current.RoadInformation.ShouldChange( this._car ) ) { this.SwitchToNextRoadElement(); }
            var stopPoint = this._car.Conductors.Current.GetStopPoint(this._car);
            this._engine.SetStopPoint(stopPoint, 0.0f);

            var carAhead = this._carAheadIterator.Get(this._car);
            if( carAhead != null )
            {
                this._engine.SetStopPoint(carAhead.Distance, carAhead.Speed);
            }
        }

        private void SwitchToNextRoadElement()
        {
            this._car.RoadElements.Current.RoadInformation.OnExit( this._car );
            this._car.RoadElements.MoveNext();
            this._car.RoadElements.Current.RoadInformation.OnEnter( this._car );
        }

        private void SwitchToNextConductor()
        {
            this._car.Conductors.Current.OnExit( this._car );
            this._car.Conductors.MoveNext();
            this._car.Conductors.Current.OnEnter( this._car );
        }
    }
}
