using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using Castle.Core.Internal;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    // NOTE We use here information that junction can only be connected with junction edge
    public class RouteJunctionReversIterator : IEnumerable<RouteElementWithDistance>
    {
        private readonly IRoadElement _current;
        private readonly IEnumerable<BelongToRouteItem> _routesToCheck;
        private readonly List<IRoadElement> _visitedRoads = new List<IRoadElement>();
        private readonly float _maxDistance;

        public RouteJunctionReversIterator( IRoadElement owner, IEnumerable<BelongToRouteItem> routesToCheck, float fromMeter )
        {
            this._current = owner;
            this._routesToCheck = routesToCheck.ToArray();
            this._maxDistance = fromMeter;
        }

        public IEnumerator<RouteElementWithDistance> GetEnumerator()
        {
            this._visitedRoads.Clear();
            this._routesToCheck.ForEach( r => this._visitedRoads.Add( r.Route.Owner ) );
            return this._routesToCheck.SelectMany( r => this.GetElements( r.Position.Clone(), r.Route, 0.0f ) ).GetEnumerator();
        }

        private IEnumerable<RouteElementWithDistance> GetElements( IRouteMark<RouteElement> current, BuildRoute owner, float distance )
        {
            if ( current.Current.RoadElement == this._current ) { return Enumerable.Empty<RouteElementWithDistance>(); }
            if ( !current.MoveBack() )
            {
                if ( distance >= this._maxDistance || this._visitedRoads.Contains( owner.Owner ) )
                {
                    // Bug, but now I will leave it as it is
                    return new[] { new RouteElementWithDistance( owner.Owner, distance ), };
                }

                this._visitedRoads.Add( owner.Owner );
                var element = this.GetElementFromThatLeadsTo( owner.Owner, current.Current.RoadElement );
                if ( element == null ) { throw new InvalidOperationException(); }
                distance += element.Length;

                return new[] { new RouteElementWithDistance( element.RoadElement, distance ), }
                    .Concat( owner.Owner.Routes.BelongToRoutes.SelectMany( r => this.GetElements( r.Position.Clone(), r.Route, distance ) ) );
            }

            distance += current.Current.Length;
            if ( distance > this._maxDistance )
            {
                return new[] { new RouteElementWithDistance( current.Current.RoadElement, distance ) };
            }

            return new[] { new RouteElementWithDistance( current.Current.RoadElement, distance ) }.Concat( this.GetElements( current, owner, distance ) );
        }

        private RouteElement GetElementFromThatLeadsTo( IRoadElement owner, IRoadElement roadElement )
        {
            return owner.Routes.AvailableRoutes.Select( r => r.Elements.FirstOrDefault( p => p.RoadElement == roadElement ) ).FirstOrDefault();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}