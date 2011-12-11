using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.CarsSpecification;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Messages;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface ICarsFactory
    {
        void CreateCar( CarsInserter startElement );
    }

    public class CarsFactory : ICarsFactory
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ICarSpecifiaction[] _carsSpecifications;
        private readonly Random _rng = new Random();
        private int _carId;

        public CarsFactory( IEventAggregator eventAggregator, IEnumerable<ICarSpecifiaction> carSpecifiactions )
        {
            Contract.Requires( eventAggregator != null ); Contract.Requires( carSpecifiactions != null ); Contract.Ensures( this._carsSpecifications.Length > 0 );
            this._eventAggregator = eventAggregator;
            this._carsSpecifications = carSpecifiactions.ToArray();
        }

        public void CreateCar( CarsInserter startElement )
        {
            if ( this._carId > 11 )
            {
                return;
            }

            this._carId++;
            if ( startElement == null ) throw new ArgumentNullException( "startElement" );

            var car = this.GetRandomCarSpecifcation().Create( startElement );
            this._eventAggregator.Publish( new CarCreated( car ) );
        }

        private ICarSpecifiaction GetRandomCarSpecifcation()
        {
            var index = this._rng.Next( 0, this._carsSpecifications.Length );
            return this._carsSpecifications[ index ];
        }

    }
}