using Microsoft.Xna.Framework;

namespace Xna
{
    public static class GraphicHelpers
    {
        public static Vector2 PrzesunPunktOOdlegloscIKat(Vector2 orginaLocation, float lenght, float angel)
        {
            var direction = Matrix.CreateTranslation(lenght, 0, 0);
            direction = direction * Matrix.CreateRotationZ(angel);
            direction = direction*Matrix.CreateTranslation(orginaLocation.X, orginaLocation.Y, 0);

            return Vector2.Transform(Vector2.Zero, direction);
        }
    }
}