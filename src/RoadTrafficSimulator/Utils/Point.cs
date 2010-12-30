using System;
using Microsoft.Xna.Framework;
using XnaVs10.Extension;

namespace XnaVs10.Utils
{
    public class Line : IEquatable<Line>
    {
        private Vector2 _begin;

        private Vector2 _end;

        public Line(Vector2 begin, Vector2 end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public Line(float ax, float ay, float bx, float by) 
            : this ( new Vector2( ax, ay ), new Vector2( bx, by ) )
        {}

        public event EventHandler LocationChanged;

        public Vector2 Begin
        {
            get { return this._begin; }
            set
            {
                this._begin = value;
                this.LocationChanged.Raise( this );
            }
        }

        public Vector2 End
        {
            get { return this._end; }
            set
            {
                this._end = value;
                this.LocationChanged.Raise(this);
            }
        }

        public float Length
        {
            get { return (Begin - End).Length(); } 
        }

        public bool Equals(Line other)
        {
            return (this.Begin == other.Begin &&
                    this.End == other.End);
        }

        public override bool Equals(object obj)
        {
            if( obj is Line )
            {
                return this.Equals((Line) obj);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Begin.GetHashCode() + this.End.GetHashCode();
        }

        public bool IsValid()
        {
            return this.Begin.IsValid() && this.End.IsValid();
        }

        public Vector2 Multiply(float scale)
        {
            var translate = this.End - this.Begin;
            var scaled = translate*scale;

            return scaled + this.Begin;
        }

        public float GetLenght()
        {
            return Vector2.Distance(this.End, this.Begin);
        }

        public Vector2 ToVector()
        {
            return this.End - this.Begin;
        }

        public void Transform(Matrix translateMatrix)
        {
            this.Begin = Vector2.Transform(this.Begin, translateMatrix);
            this.End = Vector2.Transform(this.End, translateMatrix);
        }
    }
}