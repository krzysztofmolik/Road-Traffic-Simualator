using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Road.RoadCommponets;

namespace RoadTrafficSimulator.Road
{
    public class RoadLayer : CompostControl<VertexPositionColor>
    {
        private readonly Stored _stored;
        private readonly RoadLayerVertexContainer _specifiedVertexContainer;
        private readonly Graphic _graphics;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;
        private Vector2 _location = Vector2.Zero;

        public RoadLayer(
                Stored stored,
                Graphic graphics )
            : base( null )
        {
            this._stored = stored.NotNull();

            this._specifiedVertexContainer = new RoadLayerVertexContainer( this );
            this._graphics = graphics;
            this._mouseSupport = new RoadLayerMouseSupport( this );
            this._selectionSupport = new DefaultCompositeControlSelectionSupport( this );
        }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._specifiedVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override Vector2 Location
        {
            get { return this._location; }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public IRoadLaneBlock GetRoadLineAtPoint( Vector2 point )
        {
            var roadLine = this._stored.RoadLanes.FirstOrDefault( s => s.HitTest( point ) );
            return roadLine;
        }

        public void Draw( TimeSpan timeSpan )
        {
            this._specifiedVertexContainer.Draw( this._graphics );

            this._graphics.VertexPositionalColorDrawer.Flush();
        }

        public LightsLocation GetLightPosition( Vector2 mousePosition )
        {
            var lightPosition = from roadLine in this._stored.RoadLanes
                                let block = roadLine.HitRoadBlock( mousePosition )
                                where block != null
                                select block.FirstOrDefault();

            return lightPosition.Where( t => t is LightsLocation ).FirstOrDefault() as LightsLocation;
        }

        public void SetLight( LightsLocation lightsLocation )
        {
            lightsLocation.Light = new NormalLight();
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this._location = Vector2.Transform( this._location, matrixTranslation );
            this.Children.ForEach( s => s.Translate( matrixTranslation ) );
        }

        public override bool HitTest( Vector2 point )
        {
            return false;
        }
    }
}