using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public class RoadJunctionBlockControlConverter : ControlConverterBase
    {
        public override Type Type
        {
            get { return typeof( RoadJunctionBlock ); }
        }

        public override IEnumerable<IAction> ConvertToAction( IControl control )
        {
            Debug.Assert( control is RoadJunctionBlock );
            return this.Convert( ( RoadJunctionBlock ) control );
        }

        private IEnumerable<IAction> Convert( RoadJunctionBlock control )
        {
            yield return CreateNewCommand( control );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.LeftTop.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.RightTop.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.RightBottom.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.LeftBottom.Location );
            if ( control.LeftEdge.Connector.Edge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.LeftEdge.Connector.ConnectBeginWith( Find.In( control.LeftEdge.Connector.Edge.Parent ).Property( control.LeftEdge.Connector.Edge ) ) );
            }
            if ( control.RightEdge.Connector.Edge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.RightEdge.Connector.ConnectBeginWith( Find.In( control.RightEdge.Connector.Edge.Parent ).Property( control.RightEdge.Connector.Edge ) ) );
            }
            if ( control.TopEdge.Connector.Edge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.TopEdge.Connector.ConnectBeginWith( Find.In( control.TopEdge.Connector.Edge.Parent ).Property( control.TopEdge.Connector.Edge ) ) );
            }
            if ( control.BottomEdge.Connector.Edge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.BottomEdge.Connector.ConnectBeginWith( Find.In( control.BottomEdge.Connector.Edge.Parent ).Property( control.BottomEdge.Connector.Edge ) ) );
            }

            // BUG To nie zadziala
            base.BuildRoutes( control.LeftEdge );
            base.BuildRoutes( control.RightEdge );
            base.BuildRoutes( control.TopEdge );
            base.BuildRoutes( control.BottomEdge );
        }

        private static UseCtorToCreateControl<RoadJunctionBlock> CreateNewCommand( RoadJunctionBlock control )
        {
            return Actions.CreateControl( control.Id, () => new RoadJunctionBlock( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ), Is.Const<IControl>( null ) ) );
        }
    }
}