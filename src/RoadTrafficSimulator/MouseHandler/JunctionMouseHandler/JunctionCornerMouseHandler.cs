using Microsoft.Xna.Framework;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.MouseHandler.JunctionMouseHandler;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler
{
    public class JunctionCornerMouseHandler : INestedJunctionMouseHandler
    {
        private MovablePoint _selectedCorner = null;

        public bool MouseDown( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            this._selectedCorner = junction.CornerHitTest( mouseState.Location );
            return ( this._selectedCorner != null );
        }

        public bool MouseUp( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            var result = this._selectedCorner != null;
            this._selectedCorner = null;
            return result;
        }

        public bool MouseMove( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            if ( this._selectedCorner != null )
            {
                var vectorTranslation = mouseState.Location - junction.Location;
                this._selectedCorner.Translate( Matrix.CreateTranslation( vectorTranslation.ToVector3() ) );
            }

            return this._selectedCorner != null;
        }

        public bool MouseClick( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            var corner = junction.CornerHitTest( mouseState.Location );
            if ( corner != null )
            {
                return true;
            }
            return false;
        }
    }
}