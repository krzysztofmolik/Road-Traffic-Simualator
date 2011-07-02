using System.IO;

namespace RoadTrafficSimulator.Components.BuildMode.Messages
{
    public class LoadMap
    {
        public LoadMap( Stream stream )
        {
            this.Stream = stream;
        }

        public Stream Stream { get; set; }
    }
}