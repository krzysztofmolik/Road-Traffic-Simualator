using System;
using System.Diagnostics.Contracts;
using Common.Extensions;
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class UnloadConntent
    {
        public static UnloadConntent Create<TComponent>() where TComponent : IGameComponent
        {
            return new UnloadConntent( typeof( TComponent ) );
        }

        public UnloadConntent( Type componentType )
        {
            Contract.Requires( componentType != null ); Contract.Requires( componentType.IsImplementingInterface<IGameComponent>() );
            this.ComponentType = componentType;
        }

        public Type ComponentType { get; private set; }
    }
}