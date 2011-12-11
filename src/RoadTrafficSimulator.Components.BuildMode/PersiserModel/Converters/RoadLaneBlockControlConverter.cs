using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class RoadLaneBlockControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( RoadLaneBlock ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is RoadLaneBlock );
            return this.Convert( ( RoadLaneBlock ) control );
        }

        private IEnumerable<IAction> Convert( RoadLaneBlock control )
        {
            yield return CreateNewCommand( control );
            if ( control.LeftEdge.Connector.PreviousEdge != null )
            {
                if ( control.LeftEdge.Connector.PreviousEdge is RoadJunctionEdge )
                {
                    yield return Actions.Call<RoadLaneBlock>(
                        control.Id,
                        // BUG To nie bedzie dzialac
                        () =>
                        control.LeftEdge.Connector.ConnectBeginWith( ( IControl ) Find.In( control.LeftEdge.Connector.PreviousEdge.Parent ).Property( control.LeftEdge.Connector.PreviousEdge ) ) );
                }
                else
                {
                    yield return Actions.Call<RoadLaneBlock>(
                        control.Id,
                        // BUG To nie bedzie dzialac
                        () =>
                        control.LeftEdge.Connector.ConnectBeginWith( Is.Control( control.LeftEdge.Connector.PreviousEdge.Parent ) ) );

                }
            }

            if ( control.RightEdge.Connector.NextEdge != null )
            {
                if ( control.RightEdge.Connector.NextEdge is RoadJunctionEdge )
                {
                    yield return Actions.Call<RoadLaneBlock>(
                        control.Id,
                        // BUG To nie bedzie dzialac
                        () =>
                        control.RightEdge.Connector.ConnectEndWith( ( IControl ) Find.In( control.RightEdge.Connector.NextEdge.Parent ).Property( control.RightEdge.Connector.NextEdge ) ) );
                }
                else
                {
                    yield return Actions.Call<RoadLaneBlock>(
                        control.Id,
                        // BUG To nie bedzie dzialac
                        () =>
                        control.RightEdge.Connector.ConnectEndWith( Is.Control( control.RightEdge.Connector.NextEdge.Parent ) ) );

                }
            }

            base.BuildRoutes( control.LeftEdge );
            base.BuildRoutes( control.RightEdge );
        }

        private static UseCtorToCreateControl<RoadLaneBlock> CreateNewCommand( RoadLaneBlock control )
        {
            return Actions.CreateControl( control.Id, () => new RoadLaneBlock( Is.Ioc<Factories.Factories>(), Is.Const<IControl>( null ) ) );
        }
    }
}