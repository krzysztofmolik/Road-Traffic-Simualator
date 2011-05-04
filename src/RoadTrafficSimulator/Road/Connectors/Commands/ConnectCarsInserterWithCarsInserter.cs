using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    // TODO Copy from ConnectRoadConnectionWithRoadConnection - remove logic duplication
    public class ConnectCarsInserterWithCarsInserter : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var firstConnction = first as CarsInserter;
            var secondConnection = second as CarsInserter;

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

        private bool IsHigher( CarsInserter firstConnction, CarsInserter secondConnection )
        {
            var fromStartPointDistance = Vector2.Distance( firstConnction.StartLocation, secondConnection.Location );
            var fromEndPointDistance = Vector2.Distance( firstConnction.EndLocation, secondConnection.Location );

            return fromStartPointDistance <= fromEndPointDistance;
        }

        private bool AreConnected( CarsInserter firstConnction, CarsInserter secondConnection )
        {
            var firstConnector = firstConnction.Connector;

            return firstConnector.Top == secondConnection || firstConnector.Bottom == secondConnection;
        }
    }
}