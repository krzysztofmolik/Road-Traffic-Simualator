using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Interfaces;

namespace Xna
{
    public class RectlangeDraw
    {
        private Rectangle _rectlange;
        public Color Color { get; set; }

        public float Left { get { return _rectlange.Left; } }

        public float Top { get { return _rectlange.Top; } }

        public float Right { get { return _rectlange.Right; } }

        public float Bottom { get { return _rectlange.Bottom; } }

        public RectlangeDraw(Vector2 orginVector, int width, int height)
        {
            _rectlange =  CreateRectlangeFromOrgin(orginVector, height, width);
        }

        private static Rectangle CreateRectlangeFromOrgin(Vector2 orginVector, int height, int width)
        {
            var xCordinate = (int) (orginVector.X - height/2);
            var yCordinate = (int) (orginVector.Y - width/2);

            return new Rectangle( xCordinate, yCordinate, width, height);
        }

        public bool Contains(int x, int y)
        {
            return _rectlange.Contains(x, y);
        }

        public void Draw(IPrimitiveBatch primitiveBatch)
        {
            primitiveBatch.Begin(PrimitiveType.LineList);
            primitiveBatch.AddVertex(new Vector2(_rectlange.Left, _rectlange.Top), Color );
            primitiveBatch.AddVertex(new Vector2(_rectlange.Right, _rectlange.Top), Color );

            primitiveBatch.AddVertex(new Vector2(_rectlange.Right, _rectlange.Top), Color );
            primitiveBatch.AddVertex(new Vector2(_rectlange.Right, _rectlange.Bottom), Color );

            primitiveBatch.AddVertex(new Vector2(_rectlange.Right, _rectlange.Bottom), Color );
            primitiveBatch.AddVertex(new Vector2(_rectlange.Left, _rectlange.Bottom), Color );

            primitiveBatch.AddVertex(new Vector2(_rectlange.Left, _rectlange.Bottom), Color );
            primitiveBatch.AddVertex(new Vector2(_rectlange.Left, _rectlange.Top), Color );
            primitiveBatch.End();
        }

        public void Move(int x, int y)
        {
            // TODO Create new Object :/
            var oldRectlange = _rectlange;
            _rectlange = CreateRectlangeFromOrgin(new Vector2(x, y), oldRectlange.Height, oldRectlange.Width);
        }

        public Point Center
        {
            get { return _rectlange.Center; }
        }
    }
}