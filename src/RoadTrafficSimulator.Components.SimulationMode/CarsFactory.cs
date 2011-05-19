using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.Controlers;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class CarsFactory
    {
        private readonly CarsDrawerControler _carsesDrawer;
        private readonly IEventAggregator _eventAggregator;

        public CarsFactory( CarsDrawerControler carsesDrawer, IEventAggregator eventAggregator )
        {
            Contract.Requires( carsesDrawer != null );
            Contract.Requires( eventAggregator != null );
            this._carsesDrawer = carsesDrawer;
            this._eventAggregator = eventAggregator;
        }

        public void CreateCar( CarsInserter startElement )
        {
            if ( startElement == null ) throw new ArgumentNullException( "startElement" );
            var car = new Car()
                          {
                              Velocity = 16.6666f,
                          };

            var route = this.GetRandomRoute( startElement );
            foreach ( var roadElement in route )
            {
                car.Route.Enqueue( roadElement );
            }

            startElement.Condutor.Take( car );
            this._eventAggregator.Publish( new CarCreated( car ) );
        }

        private IEnumerable<IRoadElement> GetRandomRoute( IRoadElement startElement )
        {
            var route = new List<IRoadElement>();
            route.Add( startElement );
            while ( true )
            {
                var nextElement = startElement.Condutor.GetNextRandomElement();
                if ( nextElement == null )
                {
                    break;
                }

                route.Add( nextElement );
            }

            return route;
        }
    }
}