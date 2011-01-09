using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.MouseHandler.JunctionMouseHandler;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler
{
    public class JunctionEdgeMouseHandler : INestedJunctionMouseHandler
    {
        private RoadJunctionEdge _selectedRoadJunctionEdge;
        private Vector2 _offset;

        public bool MouseDown( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            this._selectedRoadJunctionEdge = FindEdge( mouseState.Location, junction );
            if ( this._selectedRoadJunctionEdge != null )
            {
                this._offset = this.CalculateOffset( this._selectedRoadJunctionEdge, mouseState.Location );
            }
            return this._selectedRoadJunctionEdge != null;
        }

        public bool MouseUp( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            var result = this._selectedRoadJunctionEdge != null;
            this._selectedRoadJunctionEdge = null;
            return result;
        }

        public bool MouseMove( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            if ( this._selectedRoadJunctionEdge != null )
            {
                this.MoveEdge( this._selectedRoadJunctionEdge, mouseState.Location, this._offset );
                return true;
            }

            return false;
        }

        public bool MouseClick( XnaMouseState mouseState, IRoadJunctionBlock junction )
        {
            var edge = this.FindEdge( mouseState.Location, junction );
            if ( edge != null )
            {
                edge.IsSelected = true;
                return true;
            }
            return false;
        }

        private void MoveEdge( RoadJunctionEdge roadJunctionEdge, Vector2 newLocation, Vector2 offset )
        {
            var vectorTranslation = newLocation - roadJunctionEdge.StartLocation + offset;
            roadJunctionEdge.Translate( Matrix.CreateTranslation( vectorTranslation.ToVector3() ) );
        }

        private Vector2 CalculateOffset( RoadJunctionEdge roadJunctionEdge, Vector2 mousePressedPoint )
        {
            return mousePressedPoint - roadJunctionEdge.StartLocation;
        }

        private RoadJunctionEdge FindEdge( Vector2 mousePressedPoint, IRoadJunctionBlock block )
        {
            return block.RoadJunctionEdges.FirstOrDefault( s => s.HitTest( mousePressedPoint ) );
        }
    }
}