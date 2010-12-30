using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xna;
using XnaVs10.Utils;

namespace XnaVs10
{
    public class GraphicLine : ICustomDrawable
    {
        public GraphicLine(Vector2 startPoint )
        {
            this.StartPoint = startPoint;
            this.EndPoint = startPoint;
        }

        public void Draw( TimeSpan time, PrimitiveBatch primitiveBatch )
        {
            primitiveBatch.Begin(PrimitiveType.LineList);
            primitiveBatch.AddVertex(this.StartPoint, Color.Black );
            primitiveBatch.AddVertex(this.EndPoint, Color.Black );
            primitiveBatch.End();
        }

        public Vector2 EndPoint { get; set; }

        public Vector2 StartPoint { get; set; }

        public Line ToLine()
        {
            return new Line( this.StartPoint, this.EndPoint );
        }
    }
}