using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class CarsRemoverControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( CarsRemover ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is CarsRemover );
            return this.Convert( ( CarsRemover ) control );
        }

        private IEnumerable<IAction> Convert( CarsRemover control )
        {
            yield return CreateNewCommand( control );
            if ( control.Connector.ConnectedEdge != null )
            {
                yield return CallAction.Create<CarsRemover>( control.Id, () => control.Connector.ConnectBeginWith( null ), ControlProperties.Create( control.Connector.ConnectedEdge.Parent, control.Connector.ConnectedEdge ) );
            }
        }

        private static CreateControlCommand CreateNewCommand( CarsRemover control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( CarsRemover ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( Factories.Factories ) ) );
            createCommand.AddConstructParameters( Parameter.Create( control.Location ) );
            createCommand.AddConstructParameters( Parameter.Create<IControl>( null ) );
            return createCommand;
        }
    }
}