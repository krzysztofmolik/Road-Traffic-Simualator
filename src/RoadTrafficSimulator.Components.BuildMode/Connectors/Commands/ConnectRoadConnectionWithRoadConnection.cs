using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
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
}