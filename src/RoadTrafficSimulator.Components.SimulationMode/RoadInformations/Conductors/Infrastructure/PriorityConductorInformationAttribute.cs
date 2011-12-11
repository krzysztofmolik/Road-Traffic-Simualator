using System;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
    public class PriorityConductorInformationAttribute : Attribute
    {
        public PriorityConductorInformationAttribute(PriorityType priority )
        {
            this.Priority = priority;
        }

        public PriorityType Priority { get; private set; }
    }
}