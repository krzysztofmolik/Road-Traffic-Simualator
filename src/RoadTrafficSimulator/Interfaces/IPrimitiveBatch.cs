using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Interfaces
{
    public interface IPrimitiveBatch : IDisposable
    {
        void Begin(PrimitiveType primitiveType);
        void AddVertex(Vector2 vertex, Color color);
        void End();
    }
}