using System;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using System.Linq;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.Extensions
{
    public static class RoadJunctionBlockExtension
    {
        public static RoadJunctionEdge GetOpositeEdge( this RoadJunctionBlock junction, RoadJunctionEdge edge )
        {
            Debug.Assert( junction.RoadJunctionEdges.Length == EdgeType.Count );
            var idex = Array.IndexOf( junction.RoadJunctionEdges, edge );
            if ( idex < 0 ) { throw new ArgumentException(); }
            return junction.RoadJunctionEdges[ ( idex + 2 ) % 4 ];
        }
    }
}