using System;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class TextureParamter : IAction
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly string _textureName;

        public TextureParamter( string textureName)
        {
            this._textureName = textureName;
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public object Execute( DeserializationContext context )
        {
            return context.ContentManager.Load( this._textureName);
        }

        public Type Type
        {
            get { return typeof( CachedTexture ); }
        }

        public Guid CommandId
        {
            get { return this._id; }
        }
    }
}