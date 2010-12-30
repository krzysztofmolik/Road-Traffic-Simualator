using Microsoft.Xna.Framework;
namespace Xna.Extension
{
    public static class Vector3ExtensionMethod
    {
        public static Vector2 ToVector2(this Vector3 baseVector)
        {
            return new Vector2(baseVector.X, baseVector.Y);
        }
    }
}