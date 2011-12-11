using System;
using System.Linq;
using Castle.Core.Internal;
using Common.Extensions;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure
{
    public class ConductorResolver
    {
        private class ResolverItem
        {
            public Type CondcutorType { get; set; }
            public Type RouteElementType { get; set; }
            public PriorityType[] PriorityType { get; set; }
        }

        private static readonly ILookup<Type, ResolverItem> _items;

        static ConductorResolver()
        {
            var conductorBaseType = typeof( IConductor );
            _items = conductorBaseType.Assembly.GetTypes().Where( t => t.Namespace == conductorBaseType.Namespace )
                .Where( t => t.HasAttribute<ConductorSupportedRoadElementTypeAttribute>() )
                .Where( t => t.HasAttribute<PriorityConductorInformationAttribute>() )
                .Where( t => t.IsImplementingInterface<IConductor>() )
                .Select( t => new ResolverItem()
                                  {
                                      CondcutorType = t,
                                      PriorityType =
                                          t.GetAttributes<PriorityConductorInformationAttribute>().Select(
                                              p => p.Priority ).ToArray(),
                                      RouteElementType =
                                          t.GetAttribute<ConductorSupportedRoadElementTypeAttribute>().RouteElementType
                                  } )
                .ToLookup( t => t.RouteElementType, t => t );
        }

        private readonly Func<Type, IConductor> _condcutorFactory;

        public ConductorResolver( Func<Type, IConductor> condcutorFactory )
        {
            this._condcutorFactory = condcutorFactory;
        }

        public IConductor Resolve( Type routeElementType, PriorityType priorityType )
        {
            var condcutorType = GetCondcutorType( routeElementType, priorityType );
            return this._condcutorFactory( condcutorType );
        }

        private static Type GetCondcutorType( Type routeElementType, PriorityType priorityType )
        {
            var item = _items[ routeElementType ].FirstOrDefault( t => t.PriorityType.Any( p => p == priorityType ) );
            if ( item == null )
            {
                throw new ArgumentException( string.Format( "Not supported route type {0} or priority {1}", routeElementType.Name, priorityType ) );
            }

            return item.CondcutorType;
        }
    }
}