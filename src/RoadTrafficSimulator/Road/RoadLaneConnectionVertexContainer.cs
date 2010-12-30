using System;
using Common;
using Microsoft.Xna.Framework.Graphics;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road
{
    public class RoadLaneConnectionVertexContainer : IVertexContainer<VertexPositionColor>
    {
        private readonly RoadLaneConnection _roadLaneConnection;

        public RoadLaneConnectionVertexContainer( RoadLaneConnection roadLaneConnection )
        {
            this._roadLaneConnection = roadLaneConnection.NotNull();
        }

        public IShape Shape
        {
            get { throw new NotImplementedException(); }
        }

        public VertexPositionColor[] Vertex
        {
            get { throw new NotImplementedException(); }
        }

        public void Draw( Graphic graphic )
        {
            throw new NotImplementedException();
        }
    }
}