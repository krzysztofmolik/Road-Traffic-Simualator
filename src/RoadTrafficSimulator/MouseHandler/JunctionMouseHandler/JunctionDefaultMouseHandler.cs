using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.MouseHandler.JunctionMouseHandler;
using RoadTrafficSimulator.Road.Controls;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler
{
    public class JunctionDefaultMouseHandler : INestedJunctionMouseHandler
    {
        private IRoadJunctionBlock _selectedJuctnion;
        private Vector2 _offset;

        public bool MouseDown( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            this._selectedJuctnion = junction;
            this._offset = this.CalculateOffset( this._selectedJuctnion, mouseState.Location );
            return true;
        }

        private Vector2 CalculateOffset( IRoadJunctionBlock block, Vector2 mouseXnaPosition )
        {
            return mouseXnaPosition - block.LeftTopLocation;
        }

        public bool MouseUp( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            this._selectedJuctnion = null;
            return true;
        }

        public bool MouseMove( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            Debug.Assert( this._selectedJuctnion == junction );
            this.MoveBlock( this._selectedJuctnion, mouseState.Location, this._offset );
            return false;
        }

        private void MoveBlock( IRoadJunctionBlock block, Vector2 newLocation, Vector2 blockOffset )
        {
            var translationVector = newLocation - block.Location + blockOffset;
            var tranlationMatrix = Matrix.CreateTranslation( translationVector.ToVector3() );
            block.Translate( tranlationMatrix );
        }

        public bool MouseClick( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            return true;
        }
    }
}