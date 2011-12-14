using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public static class Styles
    {
        private static readonly Style _carInserterStyle = new Style { NormalColor = Color.Green, SelectionColor = Color.BlueViolet };
        private static readonly Style _carRemoverStyle = new Style { NormalColor = Color.Red, SelectionColor = Color.BlueViolet };
        private static readonly Style _normalStyle = new Style() { NormalColor = new Color( 162, 162, 162 ), SelectionColor = Color.BlueViolet };

        public static Style CarInserterStyl { get { return _carInserterStyle; } }
        public static Style CarRemoverStyle { get { return _carRemoverStyle; } }

        public static Style NormalStyle { get { return _normalStyle; } }
    }
}