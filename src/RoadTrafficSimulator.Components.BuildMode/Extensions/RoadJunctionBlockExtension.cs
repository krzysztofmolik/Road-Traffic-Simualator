using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.Extensions
{
    public static class RoadJunctionBlockExtension
    {
        public static InternalRoadJunctionEdge GetOpositeEdge( this RoadJunctionBlock junction, InternalRoadJunctionEdge edge )
        {
            Debug.Assert( junction.JunctionEdges.Length == EdgeType.Count );
            var idex = Array.IndexOf( junction.JunctionEdges, edge );
            if ( idex < 0 ) { throw new ArgumentException(); }
            return junction.JunctionEdges[ ( idex + 2 ) % 4 ];
        }
    }
}