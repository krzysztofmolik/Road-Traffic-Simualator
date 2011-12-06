using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class RoadConnectionControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( RoadConnection ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is RoadConnection );
            return this.Convert( ( RoadConnection ) control );
        }

        private IEnumerable<IAction> Convert( RoadConnection control )
        {
            yield return CreateNewCommand( control );
            yield return Actions.Call<RoadConnection>(
                control.Id,
                () => control.Connector.ConnectBeginWith( Find.In( control.Connector.PreviousConnectedEdge.Parent ).Property( control.Connector.PreviousConnectedEdge ) ) );

            yield return Actions.Call<RoadConnection>(
                control.Id,
                () => control.Connector.ConnectEndWith( Find.In( control.Connector.NextConnectedEdge.Parent ).Property( control.Connector.NextConnectedEdge ) ) );

            yield return base.BuildRoutes( control );
        }

        private static UseCtorToCreateControl<RoadConnection> CreateNewCommand( RoadConnection control )
        {
            return Actions.CreateControl( control.Id,
                                          () =>
                                          new RoadConnection( Is.Ioc<Factories.Factories>(),
                                                              Is.Const( control.Location ),
                                                              Is.Const( default( IControl ) ) ) );
        }
    }
}