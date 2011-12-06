using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class CarsRemoverControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( CarsRemover ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is CarsRemover );
            return this.Convert( ( CarsRemover ) control );
        }

        private IEnumerable<IAction> Convert( CarsRemover control )
        {
            yield return CreateNewCommand( control );
            if ( control.Connector.ConnectedEdge != null )
            {
                yield return
                    Actions.Call<CarsRemover>(
                        control.Id,
                        () => control.Connector.ConnectBeginWith( Find.In( control.Connector.ConnectedEdge.Parent ).Property( control.Connector.ConnectedEdge ) ) );
            }
        }

        private static IAction CreateNewCommand( IControl control )
        {
            return Actions.CreateControl( control.Id, () => new CarsRemover( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ), Is.Const<IControl>( null ) ) );
        }
    }
}