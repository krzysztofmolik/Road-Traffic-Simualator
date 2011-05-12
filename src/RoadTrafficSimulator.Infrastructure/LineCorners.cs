namespace RoadTrafficSimulator.Infrastructure
{
    public static class LineCorners
    {
        public const int LeftBegin = 0;
        public const int RightBegin = 1;
        public const int RightEnd = 2;
        public const int LeftEnd = 3;

        public const int Count = 4;
    }

    public static class Corners
    {
        public const int LeftTop = 0;
        public const int RightTop = 1;
        public const int RightBottom = 2;
        public const int LeftBottom = 3;

        public const int Count = 4;
    }

    public struct EdgeType
    {
        public const int Left = 0;
        public const int Top = 1;
        public const int Right = 2;
        public const int Bottom = 3;

        public const int Count = 4;

        public EdgeType( int edge ) 
            : this()
        {
            Index = edge;
        }

        public int Index { get; set; }
    }
}