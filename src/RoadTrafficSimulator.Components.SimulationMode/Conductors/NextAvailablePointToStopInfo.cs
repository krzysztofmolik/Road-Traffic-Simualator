using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class NextAvailablePointToStopInfo
    {
        public class Item
        {
            public float Left { get; private set; }
            public float Right { get; private set; }

            public Item( bool canStop, float left, float right )
            {
                CanStop = canStop;
                Left = left;
                Right = right;
            }

            public bool CanStop { get; private set; }
        }

        private List<Item> _items = new List<Item>();

        public IEnumerable<Item> Items { get { return this._items; } }

        public void AddRange( float startLocation, float endLocation, bool canStop )
        {
            this._items.Add( new Item( canStop, startLocation, endLocation ) );
        }
    }
}