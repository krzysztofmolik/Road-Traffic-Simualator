using Autofac;
using NUnit.Framework;
using RoadTrafficSimulator;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Utils.DependencyInjection;
using RoadTrafficSimulatorTests.IntegrationTests.Infacstructure;

[SetUpFixture]
public class Bootstraper
{
    [SetUp]
    public void SetUp()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule( new InfrastructureModule() );
        containerBuilder.RegisterModule( new GameModule() );
        containerBuilder.RegisterModule( new XnaCustomModule() );

        IOC.Container = containerBuilder.Build();
    }
}