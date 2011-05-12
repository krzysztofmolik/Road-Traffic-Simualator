using System;
using Common;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.MathHelpers;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class DirectionVertexContainer : IVertexContainer<VertexPositionTexture>
    {
        private readonly IRoadLaneBlock _parrent;
        private VertexPositionTexture[] _vertex;
        private Texture2D _texture;
        private Quadrangle _quadrangle;
        private const string DirectionTexture = "DirectionTexture";

        public DirectionVertexContainer( IRoadLaneBlock parrent )
        {
            this._parrent = parrent.NotNull();
            this._parrent.VectorChanged += OnParrentVertexChanged;
            this.UpdateVertex();
        }

        private void OnParrentVertexChanged( object sender, EventArgs e )
        {
            this.UpdateVertex();
        }

        private void UpdateVertex()
        {
            var line = new Line( this._parrent.BeginLocation, this._parrent.EndLocation );
            var centerLine = line.GetLenght();

            centerLine /= 2;
            var offsetFromBegin = centerLine - Constans.DistanceArrowWidth / 2;
            var offsetFromEnd = centerLine + Constans.DistanceArrowWidth / 2;

            //             TODO Zmiana nazwy
            var proporcjeOdPoczatku = offsetFromBegin / line.GetLenght();
            var proporcjeOdKonca = offsetFromEnd / line.GetLenght();
            var startPoint = line.Multiply( proporcjeOdPoczatku );
            var endPoint = line.Multiply( proporcjeOdKonca );

            var startLine = MyMathHelper.CreateTShape( startPoint, line.Begin, Constans.DistanceArrowHeight );
            var endLine = MyMathHelper.CreateTShape( endPoint, line.End, Constans.DistanceArrowHeight );


            this._quadrangle = new Quadrangle(startLine.Item1, endLine.Item2, endLine.Item1, startLine.Item2);

            var leftBottomVertex = new VertexPositionTexture( this._quadrangle.LeftBottom.ToVector3(), Vector2Ex.LeftBottom );
            var leftTopVertex = new VertexPositionTexture( this._quadrangle.LeftTop.ToVector3(), Vector2Ex.LeftTop );
            var rithtTopVertex = new VertexPositionTexture( this._quadrangle.RightTop.ToVector3(), Vector2Ex.RightTop );
            var rightBottomVertex = new VertexPositionTexture( this._quadrangle.RightBottom.ToVector3(), Vector2Ex.RightBottom );


            this._vertex = new[]
                       {
                           leftTopVertex, rithtTopVertex, leftBottomVertex,
                           rithtTopVertex, rightBottomVertex, leftBottomVertex
                       };
        }

        public VertexPositionTexture RithtTop { get; set; }

        public VertexPositionTexture RightBottom { get; set; }

        public VertexPositionTexture LeftBottom { get; set; }

        public VertexPositionTexture LeftTop { get; set; }

        public VertexPositionTexture[] Vertex
        {
            get { return this._vertex; }
        }

        public void Draw( Graphic graphic )
        {
            if ( this._texture == null )
            {
                this._texture = graphic.ContentManager.Load<Texture2D>( DirectionTexture );
            }

            graphic.VertexPositionalTextureDrawer.DrawTriangeList( this._texture, this.Vertex );
        }

        public IShape Shape
        {
            get { return this._quadrangle; }
        }
    }
}