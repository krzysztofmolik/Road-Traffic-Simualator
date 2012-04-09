using Microsoft.Xna.Framework.Graphics;

namespace Common.Xna
{
    public static class GraphicsDeviceExtension2
    {
        public static void DrawIndexedUserPrimitives<TVertex>( this GraphicsDevice graphics, TVertex[] vertices, short[] indexes )
            where TVertex : struct, IVertexType
        {
            graphics.DrawUserIndexedPrimitives( PrimitiveType.TriangleList, vertices, 0, vertices.Length, indexes, 0, indexes.Length / 3 );
        }
    }
}