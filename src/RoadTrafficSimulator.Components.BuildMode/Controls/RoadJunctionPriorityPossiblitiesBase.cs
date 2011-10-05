using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public abstract class RoadJunctionPriorityPossiblitiesBase : IPriorityPossible
    {
        public IEnumerable<PriorityType> GetPossiblePriorityTypes( IControl baseControl, IControl connectedControls )
        {
            var roadJunctionBlock = baseControl as RoadJunctionBlock;
            if ( roadJunctionBlock == null ) { return Enumerable.Empty<PriorityType>(); }

            return this.GetPossiblePriorityTypes( roadJunctionBlock, connectedControls );
        }

        protected abstract IEnumerable<PriorityType> GetPossiblePriorityTypes( RoadJunctionBlock roadJunctionBlock, IControl connectedControls );

        protected RoadJunctionEdge GetEdgeConnectedWith( RoadJunctionBlock baseConnection, IControl connectedControls )
        {
            var connectedEdge = baseConnection.RoadJunctionEdges
                .Where( s => s.Connector.Edge != null && ( s.Connector.Edge == connectedControls || s.Connector.Edge.Parent == connectedControls ) )
                .FirstOrDefault();
            if ( connectedEdge == null )
            {
                throw new ArgumentException( "Controls are not connected" );
            }
            return connectedEdge;
        }
    }
}