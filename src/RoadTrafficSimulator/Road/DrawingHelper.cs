using Common;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Road
{
    public class DrawingHelper
    {
        private const int MinCountVertexForTriangle = 3;
        private readonly GraphicsDevice _graphicsDevice;
        private Effect _effect;

        public DrawingHelper( Effect effect, GraphicsDevice graphicsDevice )
        {
            this.IsStarted = false;
            this.Effect = effect;
            this._graphicsDevice = graphicsDevice;
        }

        public Effect Effect
        {
            get { return this._effect; }
            set
            {
                value.NotNull("value");
                this._effect = value;
            }
        }

        public void DrawTriangeList<TVertex>( TVertex[] vertexs ) where TVertex : struct, IVertexType
        {
            var elementsCount = vertexs.Length / 3;

            this._graphicsDevice.DrawUserPrimitives( PrimitiveType.TriangleList, vertexs, 0, elementsCount );
        }

        private void Begin()
        {
            this.Effect.CurrentTechnique.Passes.ForEach( eP => eP.Apply());
            this.IsStarted = true;
        }

        private void End()
        {
            this.IsStarted = false;
//            this.Effect.CurrentTechnique.Passes.ForEach( eP => eP.() );
        }

        public UnitOfWork UnitOfWork
        {
            get { return new UnitOfWork( this.Begin, this.End ); }
        }

        public bool IsStarted { get; private set; }
    }
}