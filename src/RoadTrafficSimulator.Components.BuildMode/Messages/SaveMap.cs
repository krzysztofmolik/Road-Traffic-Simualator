using System.IO;

namespace RoadTrafficSimulator.Components.BuildMode.Messages
{
    public class SaveMap
    {
        public SaveMap( Stream stream )
        {
            this.Stream = stream;
        }

        public Stream Stream { get; set; }
    }
}