using Common;
using RoadTrafficSimulator.Integration;
using Xna;
using XnaVs10.Sprites;

namespace XnaVs10.Road
{
    public class LineDrawer2D
    {
        private readonly Layer2D _layer2D;
        private readonly IControlManager _controlManager;
        private GraphicLine _drawableLine;
        private bool _isEnabled;
        private readonly MessageBroker _messageBroker;

        public LineDrawer2D( Layer2D layer2D, IControlManager controlManager, MessageBroker messageBroker )
        {
            this._layer2D = layer2D.NotNull();
            this._controlManager = controlManager.NotNull();
            this._messageBroker = messageBroker.NotNull();

            this._controlManager.MousePressed += this.OnMousePressed;
            this._controlManager.MouseMove += this.OnMouseMove;
            this._controlManager.MouseReleased += this.OnMouseReleased;

//            this._messageBroker.AddRoadLane.Subscribe( this.AddRoadLane );

            this.IsEnabled = false;
        }

        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                this._isEnabled = value;
                if ( value == false )
                {
                    this.ClearLine();
                }
            }
        }

        private void OnMouseReleased( object sender, MouseStateEventArgs e )
        {
            if ( this._drawableLine == null )
            {
                return;
            }

            this._drawableLine.EndPoint = e.MousePosition;
            this._messageBroker.LineDrawed.OnNext( this._drawableLine.ToLine() );

            this.ClearLine();
        }

        private void ClearLine()
        {
            this._layer2D.Remove( this._drawableLine );
            this._drawableLine = null;
        }

        private void OnMouseMove( object sender, MouseStateEventArgs e )
        {
            if ( this._drawableLine == null )
            {
                return;
            }
            this._drawableLine.EndPoint = e.MousePosition;
        }

        private void OnMousePressed( object sender, MouseStateEventArgs e )
        {
            if ( this._drawableLine != null || !this.IsEnabled )
            {
                return;
            }

            this._drawableLine = new GraphicLine( e.MousePosition );
            this._layer2D.Add( this._drawableLine );
        }

        public void AddRoadLane( int numberOfLane )
        {
            this.IsEnabled = true;
        }
    }
}