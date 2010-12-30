using System;
using System.Diagnostics;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.VertexContainers;
using XnaVs10.Extension;
using XnaVs10.Road;

namespace RoadTrafficSimulator.Road
{
    public class LightsLocation : SingleControl<VertexPositionTexture>
    {
        private const float LightWidth = 0.05f;
        private const float LightHeight = 0.05f;

        private readonly IRoadLaneBlock _parent;
        private readonly LigthLocationVertexContainer _lightLocationVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;
        private readonly IConnectionSupport _connectionSupport;

        public LightsLocation( IRoadLaneBlock parent )
            : base( parent )
        {
            this._mouseSupport = new ControlMouseSupport( this );
            this._parent = parent.NotNull();
            this.UpdateLocation();

            this._lightLocationVertexContainer = new LigthLocationVertexContainer( this );
            this._selectionSupport = new DefaultControlSelectionSupport( this );
            this._connectionSupport = new ConnectionSupport<LightsLocation>( this );
        }

        public ILight Light
        {
            get;
            set;
        }

        public bool IsLightSet
        {
            get { return this.Light != null; }
        }

        public override IConnectionSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override Vector2 Location
        {
            get
            {
                Debug.Assert( this.Shape.Length > 0, "this.Shape.Length > 0" );
                return this.Shape.First();
            }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public float Angel
        {
            get;
            private set;
        }

        public Vector2[] Shape { get; private set; }


        public override IVertexContainer<VertexPositionTexture> SpecifiedVertexContainer
        {
            get { return this._lightLocationVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public void UpdateLocation()
        {
            this.Angel = this.CalculateAngel();
            this.Shape = new[]
                             {
                                 new Vector2( this.Location.X - LightWidth / 2, this.Location.Y - LightHeight / 2 ),
                                 new Vector2( this.Location.X + LightWidth / 2, this.Location.Y - LightHeight / 2 ),
                                 new Vector2( this.Location.X + LightWidth / 2, this.Location.Y + LightHeight / 2 ),
                                 new Vector2( this.Location.X - LightWidth / 2, this.Location.Y + LightHeight / 2 )
                             };
            this.ChangedSubject.OnNext( new Unit() );
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.Shape = this.Shape.Select( s => Vector2.Transform( s, matrixTranslation ) ).ToArray();
        }

        private float CalculateAngel()
        {
            var roadVector = this._parent.BeginLocation - this._parent.EndLocation;
            return roadVector.AngelBetween( Vector2.UnitX );
        }
    }

}