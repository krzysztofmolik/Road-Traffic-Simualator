using System;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadConnectionWithRoadConnection : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var firstConnction = first as RoadConnection;
            var secondConnection = second as RoadConnection;

            if ( firstConnction == null || secondConnection == null )
            {
                return false;
            }

            if ( firstConnction == secondConnection || this.AreConnected( firstConnction, secondConnection ) )
            {
                return false;
            }

            var isFirstHigherThanSecond = this.IsHigher( firstConnction, secondConnection );
            if ( isFirstHigherThanSecond )
            {
                firstConnction.Connector.ConnectBeginBottomWith( secondConnection );
                secondConnection.Connector.ConnectEndTopWith( firstConnction );
            }
            else
            {
                firstConnction.Connector.ConnectBeginTopWith( secondConnection );
                secondConnection.Connector.ConnectEndBottomWith( firstConnction );
            }

            return true;
        }

        private bool IsHigher( RoadConnection firstConnction, RoadConnection secondConnection )
        {
            var fromStartPointDistance = Vector2.Distance( firstConnction.StartLocation, secondConnection.Location );
            var fromEndPointDistance = Vector2.Distance( firstConnction.EndLocation, secondConnection.Location );

            return fromStartPointDistance <= fromEndPointDistance;
        }

        private bool AreConnected( RoadConnection firstConnction, RoadConnection secondConnection )
        {
            var firstConnector = firstConnction.Connector;

            return firstConnector.Top == secondConnection || firstConnector.Bottom == secondConnection;
        }
    }

    public class ConnectRoadJunctionEdge : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var firstEdge = first as RoadJunctionEdge;
            var secondEdge = second as RoadJunctionEdge;
            if ( firstEdge == null || secondEdge == null )
            {
                return false;
            }

            if ( this.HaveTheSameParent( firstEdge, secondEdge ) )
            {
                return false;
            }

            if ( this.AreConnected( firstEdge, secondEdge ) )
            {
                return false;
            }

            firstEdge.Connector.ConnectEndWith( secondEdge );
            secondEdge.Connector.ConnectBeginWith( firstEdge );

            firstEdge.RecalculatePosition();
            secondEdge.RecalculatePosition();
            return true;
        }

        private bool HaveTheSameParent( RoadJunctionEdge first, RoadJunctionEdge second )
        {
            var firstParent = first.Parent as ICompositeControl;
            if ( firstParent == null )
            {
                return false;
            }

            var theSameParent = firstParent.Children.Any( c => c == second );
            return theSameParent;
        }

        private bool AreConnected( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            return firstEdge.Connector.AreConnected( secondEdge );
        }
    }
}