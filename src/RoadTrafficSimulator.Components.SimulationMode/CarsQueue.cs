using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class CarsQueue
    {
        // TODO Change to more appropiate collection
        private readonly List<Car> _cars = new List<Car>();

        public void Add( Car car )
        {
            this._cars.Add( car );
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }

        public Car GetCarAheadOf( Car car )
        {
            // TODO Revers create new collection
            return Enumerable.Reverse( this._cars ).SkipWhile( c => c != car ).Skip( 1 ).FirstOrDefault();
        }

        public bool Contains( Car car )
        {
            return this._cars.Contains( car );
        }

        public Car GetFirstCar()
        {
            return this._cars.FirstOrDefault();
        }
    }
}