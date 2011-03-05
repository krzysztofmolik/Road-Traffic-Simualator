using Autofac;

namespace RoadTrafficSimulatorTests.IntegrationTests.Infacstructure
{
    public static class IOC
    {
        public static IContainer Container { get; set; }

        public static T GetService<T>()
        {
            return Container.Resolve<T>();
        }
    }
}