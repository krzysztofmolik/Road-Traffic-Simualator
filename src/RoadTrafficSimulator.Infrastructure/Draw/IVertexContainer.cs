﻿namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public interface IVertexContainer
    {
        void Draw( Graphic graphic );
        IShape Shape { get; }
        void ReloadTextures();
    }

    public interface IVertexContainer<out TVertex> : IVertexContainer
    {
        TVertex[] Vertex { get; }
    }
}