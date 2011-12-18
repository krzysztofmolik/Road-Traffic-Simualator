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
                .Where( t => t.IsImplementingInterface<IConductor>() )
                .Select( t => new ResolverItem()
                                  {
                                      CondcutorType = t,
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

        public IConductor Resolve( Type routeElementType )
        {
            var condcutorType = GetCondcutorType( routeElementType );
            return this._condcutorFactory( condcutorType );
        }

        private static Type GetCondcutorType( Type routeElementType )
        {
            var item = _items[ routeElementType ].FirstOrDefault();
            if ( item == null )
            {
                throw new ArgumentException( string.Format( "Not supported route type {0}", routeElementType.Name ) );
            }

            return item.CondcutorType;
        }
    }
}