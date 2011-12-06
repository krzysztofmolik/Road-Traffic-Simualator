namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public interface IParameter<out T> 
    {
        T GetValue( DeserializationContext context );
    }
}

