using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class FirstCarToOutInformation
    {
        public class Item
        {
            public Item( Car car, float carDistance )
            {
                this.Car = car;
                this.CarDistance = carDistance;
            }

            public Car Car { get; private set; }
            public float CarDistance { get; private set; }
        }

        public FirstCarToOutInformation( IEnumerable<IRoadElement> vistedControls )
        {
            this._vistedControls = new List<IRoadElement>( vistedControls );
        }

        private readonly List<Item> _items = new List<Item>();
        private readonly List<IRoadElement> _vistedControls;

        public IEnumerable<Item> Items { get { return this._items; } }
        public IEnumerable<IRoadElement> VistedElements { get { return this._vistedControls; } }

        public float CurrentDistance { get; set; }

        public void Add( Car car, float carDistance )
        {
            var item = new Item( car, carDistance );
            this._items.Add( item );
        }

        public void AddVistedControl( IRoadElement junctionEdgeConductor )
        {
            this._vistedControls.Add( junctionEdgeConductor );
        }
    }
}