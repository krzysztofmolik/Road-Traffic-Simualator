using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
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
                // Bug ??
                firstConnction.Connector.ConnectBeginBottomWith( secondConnection.Edge );
                secondConnection.Connector.ConnectEndTopWith( firstConnction.Edge );
            }
            else
            {
                // BUG ??
                firstConnction.Connector.ConnectBeginTopWith( secondConnection.Edge );
                secondConnection.Connector.ConnectEndBottomWith( firstConnction.Edge );
            }

            return true;
        }

        private bool IsHigher( RoadConnection firstConnction, RoadConnection secondConnection )
        {
            var fromStartPointDistance = Vector2.Distance( firstConnction.Edge.StartLocation, secondConnection.Location );
            var fromEndPointDistance = Vector2.Distance( firstConnction.Edge.EndLocation, secondConnection.Location );

            return fromStartPointDistance <= fromEndPointDistance;
        }

        private bool AreConnected( RoadConnection firstConnction, RoadConnection secondConnection )
        {
            var firstConnector = firstConnction.Connector;

            // BUG
            return firstConnector.Top == secondConnection.Edge || firstConnector.Bottom == secondConnection.Edge;
        }
    }
}