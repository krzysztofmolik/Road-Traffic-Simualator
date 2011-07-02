using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class CarInserterControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( CarsInserter ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
        {
            return this.Convert( ( CarsInserter ) control );
        }

        private IEnumerable<IAction> Convert( CarsInserter control )
        {
            yield return CreateNewCommand( control );

            if ( control.Connector.OpositeEdge != null )
            {
                yield return CallAction.Create<CarsInserter>( control.Id, () => control.Connector.ConnectEndWith( null ), ControlProperties.Create( control.Connector.ConnectedEdge.Parent, control.Connector.ConnectedEdge ) );
            }
        }

        private static CreateControlCommand CreateNewCommand( CarsInserter control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( CarsInserter ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( Factories.Factories ) ) );
            createCommand.AddConstructParameters( Parameter.Create( control.Location ) );
            createCommand.AddConstructParameters( Parameter.Create<IControl>( null ) );
            return createCommand;
        }
    }
}