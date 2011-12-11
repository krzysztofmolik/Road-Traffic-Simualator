using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            yield return Actions.CreateControl( control.Id, () => new RoadJunctionBlock( Is.Ioc<Factories.Factories>(), Is.Const( control.Location ), Is.Const<IControl>( null ) ) );
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

            var routesLeftEdge = control.LeftEdge.Routes.AvailableRoutes .Select( route => { var routesActions = this.BuildSingleRoute( route ); return Actions.Call<RoadJunctionBlock>( control.Id, () => control.LeftEdge.Routes.AddRoute( Is.Action<Route>( routesActions ) ) ); } ) .ToArray();
            yield return new ActionCollection( Order.Low ).AddRange( routesLeftEdge );

            var routesRightEdge = control.RightEdge.Routes.AvailableRoutes .Select( route => { var routesActions = this.BuildSingleRoute( route ); return Actions.Call<RoadJunctionBlock>( control.Id, () => control.RightEdge.Routes.AddRoute( Is.Action<Route>( routesActions ) ) ); } ) .ToArray();
            yield return new ActionCollection( Order.Low ).AddRange( routesRightEdge );

            var routesTopEdge = control.TopEdge.Routes.AvailableRoutes .Select( route => { var routesActions = this.BuildSingleRoute( route ); return Actions.Call<RoadJunctionBlock>( control.Id, () => control.TopEdge.Routes.AddRoute( Is.Action<Route>( routesActions ) ) ); } ) .ToArray();
            yield return new ActionCollection( Order.Low ).AddRange( routesTopEdge );

            var routesBottomEdge = control.BottomEdge.Routes.AvailableRoutes .Select( route => { var routesActions = this.BuildSingleRoute( route ); return Actions.Call<RoadJunctionBlock>( control.Id, () => control.BottomEdge.Routes.AddRoute( Is.Action<Route>( routesActions ) ) ); } ) .ToArray();
            yield return new ActionCollection( Order.Low ).AddRange( routesBottomEdge );
        }
    }
}