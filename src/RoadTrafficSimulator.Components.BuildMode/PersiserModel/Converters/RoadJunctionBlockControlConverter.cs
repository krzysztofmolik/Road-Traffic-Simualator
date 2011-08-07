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
    public class RoadJunctionBlockControlConverter : IControlConverter
    {
        public Type Type
        {
            get { return typeof( RoadJunctionBlock ); }
        }

        public IEnumerable<IAction> ConvertToAction( IControl control )
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
                yield return CallAction.Create<RoadJunctionBlock>( control.Id, () => control.LeftEdge.Connector.ConnectBeginWith( control.BottomEdge.Connector.Edge ), ControlProperties.Create( control.LeftEdge.Connector.Edge.Parent, control.LeftEdge.Connector.Edge ) );
                yield return SetProperties<bool>.Create<RoadJunctionBlock>( control.Id, () => control.LeftEdge.IsOut );
            }
            if ( control.RightEdge.Connector.Edge != null )
            {
                yield return CallAction.Create<RoadJunctionBlock>( control.Id, () => control.RightEdge.Connector.ConnectBeginWith( control.BottomEdge.Connector.Edge ), ControlProperties.Create( control.RightEdge.Connector.Edge.Parent, control.RightEdge.Connector.Edge ) );
                yield return SetProperties<bool>.Create<RoadJunctionBlock>( control.Id, () => control.RightEdge.IsOut );
            }
            if ( control.TopEdge.Connector.Edge != null )
            {
                yield return CallAction.Create<RoadJunctionBlock>( control.Id, () => control.TopEdge.Connector.ConnectBeginWith( control.BottomEdge.Connector.Edge ), ControlProperties.Create( control.TopEdge.Connector.Edge.Parent, control.TopEdge.Connector.Edge ) );
                yield return SetProperties<bool>.Create<RoadJunctionBlock>( control.Id, () => control.TopEdge.IsOut );
            }
            if ( control.BottomEdge.Connector.Edge != null )
            {
                yield return CallAction.Create<RoadJunctionBlock>( control.Id, () => control.BottomEdge.Connector.ConnectBeginWith( control.BottomEdge.Connector.Edge ), ControlProperties.Create( control.BottomEdge.Connector.Edge.Parent, control.BottomEdge.Connector.Edge ) );
                yield return SetProperties<bool>.Create<RoadJunctionBlock>( control.Id, () => control.BottomEdge.IsOut );
            }
        }

        private static CreateControlCommand CreateNewCommand( RoadJunctionBlock control )
        {
            var createCommand = new CreateControlCommand( control.Id, typeof( RoadJunctionBlock ) );
            createCommand.AddConstructParameters( new IocParameter( typeof( Factories.Factories ) ) );
            createCommand.AddConstructParameters( Parameter.Create( control.Location ) );
            createCommand.AddConstructParameters( Parameter.Create<IControl>( null ) );
            return createCommand;
        }
    }
}