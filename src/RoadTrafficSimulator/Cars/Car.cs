using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using Common.Xna;
using XnaRoadTrafficConstructor.Road;

namespace RoadTrafficSimulator.Cars
{
    public abstract class Car : SingleControl<VertexPositionTexture>
    {
        private CarMouseHandler _mouseHandler;
        private Vector2 _location;
        private IControl _parent;

        protected Car(IControl parent)
        {
            this._parent = parent;
            this._mouseHandler = new CarMouseHandler();
        }

        public abstract override IVertexContainer VertexContainer { get; };

        public override IMouseHandler MouseHandler { get { return this._mouseHandler;  } }

        public override void Translate(Matrix matrixTranslation)
        {
            this.TranslateWithoutNotification(matrixTranslation);
            this.Invalidate();
        }

        public override void TranslateWithoutNotification(Matrix translationMatrix)
        {
            var newLocation = Vector2.Transform(this.Location, translationMatrix);
            if(newLocation.Equal(newLocation, Constans.CarMoveEpsilon)) { return; }

            this._location = newLocation;
        }

        public override Vector2 Location
        {
            get { return this._location; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }
    }
}