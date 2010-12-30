using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public abstract class ConnectionCommandBase : IConnectionCommand
    {
        public abstract bool CanConnect( IControl first, IControl second );
        public abstract void Connect( IControl first, IControl second );

        protected T GetSpecifyType<T>( IControl first, IControl second ) where T : class
        {
            var firstAsT = first as T;
            if ( firstAsT != null )
            {
                return firstAsT;
            }

            return second as T;
        }

    }

    public class ConnectEdgeByMoveJunction : IConnectionCommand
    {
        public void Connect( IControl first, IControl second )
        {
            var firstEdge = first as RoadJunctionEdge;
            var secondEdge  = second as RoadJunctionEdge;
            if ( firstEdge == null || secondEdge == null )
            {
                return;
            }

            this.MoveSecondJunction( firstEdge, secondEdge );

            this.MixPoints( firstEdge, secondEdge );
        }

        public bool CanConnect( IControl first, IControl second )
        {
            var firstEdge = first as RoadJunctionEdge;
            var secondEdge = second as RoadJunctionEdge;

            if ( firstEdge == null || secondEdge == null )
            {
                return false;
            }

            Debug.Assert( firstEdge.Parents.Count() != 0, "firstEdge.Parents.Count() != 0" );
            Debug.Assert( secondEdge.Parents.Count() != 0, "secondEdge.Parents.Count() != 0" );
            var firstParent = firstEdge.Parents.OfType<ICompostControlBase>().FirstOrDefault();
            if ( firstParent == null )
            {
                return false;
            }

            var theSameParent = firstParent.Children.Any( c => c == secondEdge );
            return !theSameParent;
        }

        private void MoveSecondJunction( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            var secondParent = secondEdge.RoadJunctionParent;

            var firstEdgeCanter = firstEdge.StartLocation + ( ( firstEdge.EndLocation - firstEdge.StartLocation ) / 2 );
            var secondEdgeCenter = secondEdge.StartLocation + ( ( secondEdge.EndLocation - secondEdge.StartLocation ) / 2 );
            var diff = firstEdgeCanter - secondEdgeCenter;

            secondParent.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }

        private void MixPoints( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            firstEdge.StartPoint.Changed.Subscribe( s => secondEdge.EndPoint.SetLocation( firstEdge.StartPoint.Location ) );
            firstEdge.EndPoint.Changed.Subscribe( s => secondEdge.StartPoint.SetLocation( firstEdge.EndPoint.Location ) );

            secondEdge.StartPoint.Changed.Subscribe( s => firstEdge.EndPoint.SetLocation( secondEdge.StartPoint.Location ) );
            secondEdge.EndPoint.Changed.Subscribe( s => firstEdge.StartPoint.SetLocation( secondEdge.EndPoint.Location ) );
        }
    }
}