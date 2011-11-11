using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.MathHelpers;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class RoadJunctionBlockVertexContainer : VertexContainerBase<IRoadJunctionBlock, VertexPositionColor>
    {
        private Quadrangle _shape;
        private readonly List<TextureInPoint> _textures;

        public RoadJunctionBlockVertexContainer( IRoadJunctionBlock block, Style style )
            : base( block, style.NormalColor )
        {
            this._textures = new List<TextureInPoint>();
        }

        private Quadrangle CreateShape()
        {
            return new Quadrangle(

                                  this.Object.LeftBottomLocation,
                                  this.Object.RightBottomLocation,
                                  this.Object.RightTopLocation,
                                  this.Object.LeftTopLocation );
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateShape();
            this._textures.Clear();

            // TODO Draw something
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Left ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.LeftArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Right ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.RightArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Top ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.UpArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Bottom ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.DownArrow ) ); }
            var angel = Vector2.Zero.Angel();
            var location = MyMathHelper.LineIntersectionMethod( this._shape.LeftTop, this._shape.RightBottom, this._shape.RightTop, this._shape.LeftBottom );
            this._textures.ForEach( s =>
                                        {
                                            s.SetAngel( angel );
                                            s.SetLoactoin( location );
                                            s.SetWidth( Constans.RoadHeight );
                                            s.SetHeigth( Constans.RoadHeight );
                                        } );



            return this._shape.DrawableShape
                                .Select( s => new VertexPositionColor( s.ToVector3(), this.Color ) )
                                .ToArray();
        }

        public override void ReloadTextures()
        {
            this._textures.Clear();

            // Draw something
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Left ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.LeftArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Right ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.RightArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Top ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.UpArrow ) ); }
//            if ( this.Object.RoadJunctionEdges[ EdgeType.Bottom ].IsOut ) { this._textures.Add( this._textureManager.GetTextureInPoint( TextureType.DownArrow ) ); }
            var angel = Vector2.Zero.Angel();
            var location = MyMathHelper.LineIntersectionMethod( this._shape.LeftTop, this._shape.RightBottom, this._shape.RightTop, this._shape.LeftBottom );
            this._textures.ForEach( s =>
                                        {
                                            s.SetAngel( angel );
                                            s.SetLoactoin( location );
                                            s.SetWidth( Constans.RoadHeight );
                                            s.SetHeigth( Constans.RoadHeight );
                                        } );

        }


        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
            this._textures.ForEach( t => graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList( t.Texture, t.Blocks, t.Indexes ) );
            this.Object.RoadJunctionEdges.ForEach( s => s.VertexContainer.Draw( graphic ) );
            this.Object.Points.ForEach( s => s.VertexContainer.Draw( graphic ) );
        }
    }
}