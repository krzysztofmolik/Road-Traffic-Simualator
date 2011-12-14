using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    // TODO Copy from ConnectCarsInserterWithCarsInserter - remove logic duplication
    public class ConnectCarsRemoverWithCarsRemover : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var firstConnction = first as CarsRemover;
            var secondConnection = second as CarsRemover;

            if ( firstConnction == null || secondConnection == null ) { return false; }
            if ( firstConnction == secondConnection || this.AreConnected( firstConnction, secondConnection ) ) { return false; }

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

        private bool IsHigher( CarsRemover firstConnction, CarsRemover secondConnection )
        {
            var fromStartPointDistance = Vector2.Distance( firstConnction.Edge.StartLocation, secondConnection.Location );
            var fromEndPointDistance = Vector2.Distance( firstConnction.Edge.EndLocation, secondConnection.Location );

            return fromStartPointDistance <= fromEndPointDistance;
        }

        private bool AreConnected( CarsRemover firstConnction, CarsRemover secondConnection )
        {
            var firstConnector = firstConnction.Connector;
            return firstConnector.Top == secondConnection || firstConnector.Bottom == secondConnection;
        }
    }
}