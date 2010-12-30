using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.MouseHandler.JunctionMouseHandler;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler
{
    public class JuntionMouseHandlerComposite : IMouseHandler
    {
        private readonly INestedJunctionMouseHandler[] _nestedMouseHandler;

        private IRoadJunctionBlock _selectedBlock;

        public JuntionMouseHandlerComposite()
        {
            this._nestedMouseHandler = new INestedJunctionMouseHandler[]
                                           {
                                               new JunctionCornerMouseHandler(),
                                               new JunctionEdgeMouseHandler(),
                                               new JunctionDefaultMouseHandler()
                                           };
        }

        public bool MouseDown( XnaMouseState mouseState )
        {
            this._selectedBlock = this.FindBlockAtLocation( mouseState.Location );
            if ( this._selectedBlock != null )
            {
                this._nestedMouseHandler.FirstOrDefault( n => n.MouseDown( mouseState, this._selectedBlock ) );
            }
            return this._selectedBlock != null;
        }


        public bool MouseClick( XnaMouseState mouseState )
        {
            var block = this.FindBlockAtLocation( mouseState.Location );
            if ( block != null )
            {
                this._nestedMouseHandler.FirstOrDefault( n => n.MouseClick( mouseState, block ) );
            }
            return block != null;
        }

        public bool MouseMove( XnaMouseState mouseState )
        {
            if ( this._selectedBlock != null )
            {
                this._nestedMouseHandler.FirstOrDefault( n => n.MouseMove( mouseState, this._selectedBlock ) );
                return true;
            }
            return false;
        }

        public bool MouseUp( XnaMouseState mouseState )
        {
            if( this._selectedBlock != null )
            {
                this._nestedMouseHandler.FirstOrDefault(n => n.MouseUp(mouseState, this._selectedBlock));
                this._selectedBlock = null;
                return true;
            }
            return false;
        }

        private IRoadJunctionBlock FindBlockAtLocation( Vector2 location )
        {
//            return this._stored.RoadJunctionBlock.FirstOrDefault( s => s.HitTest( location ) );
            return null;
        }
    }
}