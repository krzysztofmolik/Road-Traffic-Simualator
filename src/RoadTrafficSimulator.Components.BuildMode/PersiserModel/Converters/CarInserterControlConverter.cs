using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class CarInserterControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( CarsInserter ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            return this.Convert( ( CarsInserter ) control );
        }

        private IEnumerable<IAction> Convert( CarsInserter control )
        {
            yield return CreateNewCommand( control );

            if ( control.Connector.OpositeEdge != null )
            {
                yield return Actions.Call<CarsInserter>(
                            control.Id,
                            () => control.Connector.ConnectEndWith( Find.In( control.Connector.ConnectedEdge.Parent ).Property( control.Connector.ConnectedEdge ) ) );
            }

            yield return this.BuildRoutes( control );
        }

        private static IAction CreateNewCommand( IControl control )
        {
            return Actions.CreateControl( control.Id, () => new CarsInserter( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ), Is.Const<IControl>( null ) ) );
        }
    }
}