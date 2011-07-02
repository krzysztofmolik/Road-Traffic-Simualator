using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class RoadLaneBlockControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( RoadLaneBlock ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is RoadLaneBlock );
            return this.Convert( ( RoadLaneBlock ) control );
        }

        private IEnumerable<IAction> Convert( RoadLaneBlock control )
        {
            yield return CreateNewCommand( control );
            if ( control.LeftEdge.Connector.PreviousEdge != null )
            {
                yield return CallAction.Create<RoadLaneBlock>( control.Id, () => control.LeftEdge.Connector.ConnectBeginWith( ( IEdge ) null ), ControlProperties.Create( control.LeftEdge.Connector.PreviousEdge.Parent, control.LeftEdge.Connector.PreviousEdge ) );
            }

            if ( control.RightEdge.Connector.NextEdge != null )
            {
                yield return CallAction.Create<RoadLaneBlock>( control.Id, () => control.RightEdge.Connector.ConnectEndWith( ( IEdge ) null ), ControlProperties.Create( control.RightEdge.Connector.NextEdge.Parent, control.RightEdge.Connector.NextEdge ) );
            }
        }

        private static CreateControlCommand CreateNewCommand( RoadLaneBlock control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( RoadLaneBlock ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( Factories.Factories ) ) );
            createCommand.AddConstructParameters( Parameter.Create<IControl>( null ) );
            return createCommand;
        }
    }
}