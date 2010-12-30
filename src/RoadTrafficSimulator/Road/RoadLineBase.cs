using System;
using Microsoft.Xna.Framework;
using Xna.MathHelpers;
using Xna.Road;
using Xna.Utils;
using XnaVs10.Road.RoadJoiners;
using XnaVs10.Utils;

namespace XnaVs10.Road
{
    public abstract class RoadLineBase : IRoadLine
    {
        public IRoadConnection Begin { get; protected set; }

        public IRoadConnection End { get; protected set; }

        public abstract Line LeftLine { get; protected set; }

        public abstract Line RightLine { get; protected set; }

        public virtual Vector2 BeginLocation
        {
            get { return ( ( this.RightLine.Begin - this.LeftLine.Begin ) * 0.5f ) + this.LeftLine.Begin; }
        }

        public virtual Vector2 EndLocation
        {
            get { return ( ( this.RightLine.End - this.LeftLine.End ) * 0.5f ) + this.LeftLine.End; }
        }

        public abstract void UpdateLines();

        public bool HitTest(Vector2 point)
        {
            var result = new HitTestAlghoritm().HitTest2(
                point,
                this.LeftLine.Begin,
                this.LeftLine.End,
                this.RightLine.End,
                this.RightLine.Begin,
                this.LeftLine.Begin );
            return result;
        }

        public event EventHandler VectorChanged;

        protected void RaiseVectorChanged()
        {
            if ( this.VectorChanged != null )
            {
                this.VectorChanged( this, EventArgs.Empty );
            }
        }

        private static bool IsOnTheRight(Vector2 lineBegin, Vector2 lineEnd, Vector2 point)
        {
            var line = lineEnd - lineBegin;
            var pointInZero = point - lineBegin;
            var angelLine = MyMathHelper.To360( Math.Atan2( line.X, line.Y ) );
            var angelPoint = MyMathHelper.To360( Math.Atan2( pointInZero.X, pointInZero.Y ) );
            var z = angelLine > Math.PI ? angelLine - Math.PI : angelLine + Math.PI;

            return angelPoint > Math.Max( z, angelLine ) || angelPoint < Math.Min( z, angelLine );
        }
    }
}