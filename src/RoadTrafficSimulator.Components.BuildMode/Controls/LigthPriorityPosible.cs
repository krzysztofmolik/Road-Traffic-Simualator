using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LigthPriorityPosible : RoadJunctionPriorityPossiblitiesBase
    {
        protected override IEnumerable<PriorityType> GetPossiblePriorityTypes( RoadJunctionBlock roadJunctionBlock, IControl connectedControls )
        {
            var edge = this.GetEdgeConnectedWith( roadJunctionBlock, connectedControls );
            if ( edge.Connector.Light != null )
            {
                return new[] { PriorityType.Light };
            }

            return Enumerable.Empty<PriorityType>();
        }
    }
}