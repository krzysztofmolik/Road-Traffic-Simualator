using System;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class ConductorSupportedRoadElementTypeAttribute : Attribute
    {
        public ConductorSupportedRoadElementTypeAttribute( Type routeElementType )
        {
            this.RouteElementType = routeElementType;
        }

        public Type RouteElementType { get; private set; }
        
    }
}