using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public interface IVertexContainer
    {
        void Draw( Graphic graphic );
        IShape Shape { get; }
        Color Color { get; set; }
        void ReloadTextures();
        void ClearColor();
    }

    public interface IVertexContainer<out TVertex> : IVertexContainer
    {
        TVertex[] Vertex { get; }
    }
}