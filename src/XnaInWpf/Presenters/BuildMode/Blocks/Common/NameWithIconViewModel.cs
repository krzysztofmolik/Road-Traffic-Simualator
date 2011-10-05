namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Common
{
    public class NameWithIconViewModel
    {
        public NameWithIconViewModel( string name, string icon)
        {
            this.Name = name;
            this.ImageSources = icon;
        }
        public string Name { get; private set; }
        public string ImageSources { get; private set; }
    }
}