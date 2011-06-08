using System;
using System.Diagnostics.Contracts;
using Common.Extensions;
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class IntializeContent
    {
        public static IntializeContent Create<TComponent>() where TComponent : IGameComponent
        {
            return new IntializeContent( typeof( TComponent ) );
        }

        public IntializeContent( Type componentType )
        {
            Contract.Requires( componentType != null ); Contract.Requires( componentType.IsImplementingInterface<IGameComponent>() );
            this.ComponentType = componentType;
        }

        public Type ComponentType { get; private set; }
    }
}