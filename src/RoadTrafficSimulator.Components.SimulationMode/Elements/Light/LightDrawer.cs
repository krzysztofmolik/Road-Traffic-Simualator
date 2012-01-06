using System;
using Microsoft.Xna.Framework;
using NLog;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Light
{
    public class LightDrawer : IDrawer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Light _light;
        private LightState _lightState;
        private ColorableShape _lightShape;

        public LightDrawer( Light light )
        {
            this._light = light;
            this.CreateLightShape( light.BuildControl.Location );
        }

        private void CreateLightShape( Vector2 location )
        {
            var width = Constans.ToVirtualUnit( 0.5f );
            var height = Constans.ToVirtualUnit( 0.5f );
            this._lightShape = new ColorableShape( Quadrangle.Create( location, width, height ), Color.Black );
            this.SetLightColor();
        }

        private void UpdateLightState()
        {
            if( this._lightState == this._light.LightState ) { return;   }
            this._lightState = this._light.LightState;
            this.SetLightColor();
        }

        private void SetLightColor()
        {
            switch( this._lightState )
            {
                case LightState.Red:
                    this._lightShape.SetColor( Color.Red );
                    break;
                case LightState.YiellowFromGreen:
                case LightState.YiellowFromRed:
                    this._lightShape.SetColor( Color.Yellow );
                    break;
                case LightState.Green:
                    this._lightShape.SetColor( Color.Green );
                    break;
                default:
                    Logger.Warn( "Invalid light state" );
                    throw new ArgumentException( "Invalid light state" );
            }
        }

        public void Draw( Graphic graphic, GameTime gameTime )
        {
            this.UpdateLightState();
            graphic.VertexPositionalColorDrawer.DrawIndexedTriangeList( this._lightShape.Vertex, this._lightShape.Indexes );
        }
    }
}