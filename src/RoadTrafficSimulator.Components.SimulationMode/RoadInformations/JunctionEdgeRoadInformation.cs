using System.Diagnostics;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;
using JunctionEdge = RoadTrafficSimulator.Components.SimulationMode.Elements.JunctionEdge;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class JunctionEdgeRoadInformation : RoadInformationBase, IRoadInformation
    {
        private readonly JunctionEdge _juctionEdge;

        public JunctionEdgeRoadInformation( JunctionEdge juctionEdge, IEventAggregator eventAggregator )
        {
            Contract.Requires( juctionEdge != null );
            Contract.Requires( eventAggregator != null );
            this._juctionEdge = juctionEdge;
        }

        protected override Vector2 GetBeginLocation()
        {
            return this._juctionEdge.EdgeBuilder.Location;
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            return Constans.PointSize;
        }

        public bool CanStop( IRoadElement previous, IRoadElement next )
        {
            return false;
        }

        public bool ShouldChange( Car car )
        {
            // TODO BUG?
            return true;
        }

        public FirstCarToOutInformation GetFirstCarToOutInformation()
        {
            Debug.Assert( false );
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return car.Direction;
            //            return nextPoint.BuildControl.Location - car.Location;
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            return Constans.PointSize;
        }
    }
}