using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using Microsoft.Practices.ServiceLocation;
using RoadTrafficConstructor;
using XnaInWpf.Presenters.Interfaces;
using Module = Autofac.Module;
using System.Linq;
using IEventAggregator = Common.IEventAggregator;

namespace XnaInWpf
{
    public class AutofacBootstrapper : Caliburn.Micro.Bootstrapper<IShellViewModel>
    {
        private IContainer _container;

        private IContainer BuildContainer()
        {
            var autofacBuilder = new ContainerBuilder();
            autofacBuilder.Register( c => this._container ).As<IContainer>().SingleInstance();
            autofacBuilder.RegisterType<ServiceProviderAdapter>().As<IServiceProvider>();

            autofacBuilder.RegisterModule( new CaliburnMicroModule() );
            autofacBuilder.RegisterModule( new PresentersModule() );
            autofacBuilder.RegisterModule( new XnaInWpfModule() );

            return autofacBuilder.Build();
        }

        protected override void Configure()
        {
            this._container = this.BuildContainer();
        }

        protected override object GetInstance( Type service, string key )
        {
            if ( string.IsNullOrEmpty( key ) )
            {
                return this._container.Resolve( service );
            }

            return this._container.ResolveNamed( key, service );
        }

        protected override IEnumerable<object> GetAllInstances( Type service )
        {
            var enumerableType = typeof( IEnumerable<> ).MakeGenericType( service );
            return ( IEnumerable<object> ) this._container.Resolve( enumerableType );
        }
    }

    public class ServiceProviderAdapter : IServiceProvider
    {
        private readonly IContainer _container;

        public ServiceProviderAdapter( IContainer container )
        {
            _container = container;
        }

        public object GetService( Type serviceType )
        {
            return this._container.Resolve( serviceType );
        }
    }

    public class CaliburnMicroModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<WindowManager>().As<IWindowManager>();
        }
    }

    public class PresentersModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( Assembly.GetExecutingAssembly() )
                .Where( c => c.Name.EndsWith( "Model" ) ).As( c => this.GetDefaultInterface( c ) );
        }

        private Type GetDefaultInterface( Type type )
        {
            var interfaceType = type.GetInterfaces()
                                    .Where( c => c.Name.EndsWith( "Model" ) )
                                    .FirstOrDefault();
            return interfaceType ?? type;
        }
    }
}