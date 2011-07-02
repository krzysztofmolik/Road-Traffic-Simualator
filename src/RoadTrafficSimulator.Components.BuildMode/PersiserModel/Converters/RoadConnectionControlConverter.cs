using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class RoadConnectionControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( RoadConnection ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is RoadConnection );
            return this.Convert( ( RoadConnection ) control );
        }

        private IEnumerable<IAction> Convert( RoadConnection control )
        {
            yield return CreateNewCommand( control );
            yield return CallAction.Create<RoadConnection>( control.Id, () => control.Connector.ConnectBeginWith( null ), ControlProperties.Create( control.Connector.PreviousConnectedEdge.Parent, control.Connector.PreviousConnectedEdge ) );
            yield return CallAction.Create<RoadConnection>( control.Id, () => control.Connector.ConnectEndWith( null ), ControlProperties.Create( control.Connector.NextConnectedEdge.Parent, control.Connector.NextConnectedEdge ) );
        }

        private static CreateControlCommand CreateNewCommand( RoadConnection control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( RoadConnection ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( Factories.Factories ) ) );
            createCommand.AddConstructParameters( Parameter.Create( control.Location ) );
            createCommand.AddConstructParameters( Parameter.Create<IControl>( null ) );
            return createCommand;
        }
    }
}