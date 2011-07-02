using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class LightControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( LightBlock ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
        {
            return this.Convert( ( LightBlock ) control );
        }

        private IEnumerable<IAction> Convert( LightBlock control )
        {
            yield return CreateNewCommand( control );

            Debug.Assert( control.Connector.Owner != null );
            yield return CallAction.Create<LightBlock>( control.Id, () => control.Connector.ConnectWith( null ), ControlProperties.Create( control.Connector.Owner.Parent, control.Connector.Owner ) );
        }

        private static CreateControlCommand CreateNewCommand( LightBlock control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( LightBlock ) );
            createCommand.AddConstructParameters( Parameter.Create( control.Location ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( IContentManager ) ) );
            return createCommand;
        }
    }
}