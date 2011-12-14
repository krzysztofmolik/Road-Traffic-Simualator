using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class JunctionEdgeControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( JunctionEdge ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is JunctionEdge );
            return this.Convert( ( JunctionEdge ) control );
        }

        private IEnumerable<IAction> Convert( JunctionEdge control )
        {
            yield return CreateNewCommand( control );
            yield return Actions.Call<JunctionEdge>(
                control.Id,
                () => control.Connector.ConnectWithJunction( (RoadJunctionBlock)Is.Control( control.Connector.JunctionEdge.Parent ), Is.Const( control.Connector.JunctionEdge.EdgeIndex ) ) ); 

            if( control.Connector.Edge != null )
            {
                yield return Actions.Call<JunctionEdge>(
                    control.Id,
                    () => control.Connector.ConnectBeginFrom( (RoadLaneBlock) Is.Control( control.Connector.Edge.Parent ) ) );
            }

            yield return base.BuildRoutes( control );
        }

        private static UseCtorToCreateControl<JunctionEdge> CreateNewCommand( JunctionEdge control )
        {
            return Actions.CreateControl( control.Id,
                                          () =>
                                          new JunctionEdge( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ) ) );
        }
    }
}