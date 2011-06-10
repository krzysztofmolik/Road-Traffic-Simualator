using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using System.Linq;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class LaneJuctionConductor : IConductor
    {
        private readonly List<Car> _cars = new List<Car>();
        private LaneJunction _laneJunction;

        public LaneJuctionConductor( LaneJunction laneJunction )
        {
            Contract.Requires( laneJunction != null );
            this._laneJunction = laneJunction;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route )
        {
            var edge = this._laneJunction.Edges.Where( e => e != null )
                                           .Where( e => e.Lane != null )
                                           .Where( e => e.Lane.Next != this._laneJunction )
                                           .Where( e => e != route.Last() )
                                           .FirstOrDefault();
            Debug.Assert( edge != null );
            return edge.Lane;
        }

        public void Take( Car car )
        {
            this._cars.Add( car );
        }

        public bool SholdChange( Vector2 acutalCarLocation, Car car )
        {
            var next = this._laneJunction.Edges.Where( s => s.Lane == car.Route.First() ).FirstOrDefault();
            var distance = next.BuildControl.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public LightInfomration GetLightInformation()
        {
            return new LightInfomration
                       {
                           LightDistance = float.MaxValue,
                       };
        }

        public JunctionInformation GetNextJunctionInformation()
        {
            return new JunctionInformation { JunctionDistance = float.MaxValue };
        }

        public CarInformation GetCarAheadDistance()
        {
            return new CarInformation { CarDistance = float.MaxValue };
        }

        public Vector2 GetCarDirection( Car car )
        {
            var edge = this._laneJunction.Edges.Where( e => e != null )
                                           .Where( e => e.Lane != null )
                                           .Where( e => e.Lane.Prev == this._laneJunction )
                                           .FirstOrDefault();
            return edge.BuildControl.Location - car.Location;
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }
    }
}