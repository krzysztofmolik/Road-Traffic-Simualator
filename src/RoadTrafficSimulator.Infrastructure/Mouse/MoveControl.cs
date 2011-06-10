using Common.Xna;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class MoveControl
    {
        private readonly SelectedControls _selectedControls;

        public MoveControl( SelectedControls selectedControls )
        {
            this._selectedControls = selectedControls;
        }

        public void Translate( IControl control, Vector2 translationVector )
        {
            if ( translationVector.Equal( Vector2.Zero, Constans.Epsilon ) )
            {
                return;
            }

            var translationMatrix = Matrix.CreateTranslation( translationVector.ToVector3() );
            if ( this._selectedControls.Contains( control ) )
            {
                this._selectedControls.ForEach( s => s.TranslateWithoutNotification( translationMatrix ) );
                this._selectedControls.ForEach( s => s.Invalidate() );
            }
            else
            {
                control.Translate( translationMatrix );
            }
        }
    }
}