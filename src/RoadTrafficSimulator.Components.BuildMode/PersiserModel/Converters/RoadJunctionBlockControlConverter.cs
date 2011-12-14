using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure;
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
            yield return Actions.CreateControl( control.Id, () => new RoadJunctionBlock( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ) ) );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.LeftTop.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.RightTop.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.RightBottom.Location );
            yield return SetProperties<Vector2>.Create<RoadJunctionBlock>( control.Id, () => control.LeftBottom.Location );
            if ( control.Connector.LeftEdge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.Connector.ConnectStartOn( Is.Control( control.Connector.LeftEdge ), Is.Const( EdgeType.Left ) ) );
            }
            if ( control.Connector.RightEdge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.Connector.ConnectStartOn( Is.Control( control.Connector.RightEdge ), Is.Const( EdgeType.Right ) ) );
            }

            if ( control.Connector.TopEdge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.Connector.ConnectStartOn( Is.Control( control.Connector.TopEdge ), Is.Const( EdgeType.Top ) ) );
            }
            if ( control.Connector.BottomEdge != null )
            {
                yield return Actions.Call<RoadJunctionBlock>(
                            control.Id,
                            () => control.Connector.ConnectStartOn( Is.Control( control.Connector.BottomEdge ), Is.Const( EdgeType.Bottom ) ) );
            }
        }
    }
}