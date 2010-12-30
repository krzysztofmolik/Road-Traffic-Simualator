using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xna
{
    public class TextureInfo
    {
        public Vector2 Location { get; set;}
        public Vector2 OrginVector { get; set; }
        public Rectangle? SourceRect { get; set; }

        public TextureInfo(Texture2D texture2D)
        {
            this.Texture = texture2D;
            OrginVector = new Vector2( this.Texture.Width / 2, this.Texture.Height / 2 );
            Location = Vector2.Zero;
            Angel = 0.0f;
            SourceRect = null;
        }

        public Texture2D Texture { get; set; }
        public float Angel { get; set; }

        public void PrzesunWzdlozOsiX(float odleglosc)
        {
            this.Location = GraphicHelpers.PrzesunPunktOOdlegloscIKat(this.Location, odleglosc, Angel);
        }
    }
}