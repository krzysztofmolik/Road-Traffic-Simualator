using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class LaneRoadInforamtion : RoadInformationBase, IRoadInformation
    {
        private readonly Lane _lane;

        public LaneRoadInforamtion( Lane lane )
        {
            Contract.Requires( lane != null );
            this._lane = lane;
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            if ( this._lane.Prev != previous || this._lane.Next != next )
            {
                throw new ArgumentNullException();
            }

            return Vector2.Distance( this._lane.Prev.BuildControl.Location, this._lane.Next.BuildControl.Location );
        }

        public bool CanStop( IRoadElement previous, IRoadElement next )
        {
            return true;
        }

        public bool ShouldChange( Car car )
        {
            var distance = this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            Debug.Assert( this._lane.Next == nextPoint );

            return Vector2.Distance( car.Location, nextPoint.BuildControl.Location );
        }

        public IEnumerable<IRoadElement> ReversConnection
        {
            get { throw new NotImplementedException(); }
        }

        protected override Vector2 GetEndLocation()
        {
            return this._lane.RoadLaneBlock.RightEdge.Location;
        }

        protected override Vector2 GetBeginLocation()
        {
            return this._lane.RoadLaneBlock.LeftEdge.Location;
        }
    }
}