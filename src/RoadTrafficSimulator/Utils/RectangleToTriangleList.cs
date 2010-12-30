
namespace XnaRoadTrafficConstructor.Utils
{
    public class RectangleToTriangleList
    {
        public static TVertex[] Convert<TVertex>( TVertex leftTop, TVertex rightTop, TVertex rightBottom, TVertex leftBottom )
        {
            return new[]
                       {
                           leftBottom, leftTop, rightTop,
                           rightTop, rightBottom, leftBottom
                       };
        }
    }
}