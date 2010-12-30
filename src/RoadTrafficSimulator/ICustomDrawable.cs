using System;

namespace Xna
{
    public interface ICustomDrawable
    {
        void Draw( TimeSpan time, PrimitiveBatch primitiveBatch );
    }
}