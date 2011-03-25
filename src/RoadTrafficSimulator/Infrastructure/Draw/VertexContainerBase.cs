using System;
using System.Diagnostics;
using Common;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public abstract class VertexContainerBase<T, TVertex> : IVertexContainer<TVertex> where T : class, IControl
    {
        protected VertexContainerBase( T @object )
        {
            this.Object = @object.NotNull();
            this.Object.Redrawed.Subscribe( s => this.Vertex = this.UpdateShapeAndCreateVertex() );
        }

        public T Object { get; private set; }

        public TVertex[] Vertex { get; private set; }

        public abstract IShape Shape { get; }

        public void Draw( Graphic graphic )
        {
            if ( this.Vertex == null )
            {
                this.Vertex = this.UpdateShapeAndCreateVertex();
                Debug.Assert( this.Vertex != null, "this.Vertex != null" );
            }

            this.DrawControl( graphic );
        }

        protected abstract TVertex[] UpdateShapeAndCreateVertex();

        protected abstract void DrawControl( Graphic graphic );

        protected void DrawControl( IControl controlBase, Graphic graphic )
        {
            if ( controlBase != null )
            {
                controlBase.VertexContainer.Draw( graphic );
            }
        }
    }
}