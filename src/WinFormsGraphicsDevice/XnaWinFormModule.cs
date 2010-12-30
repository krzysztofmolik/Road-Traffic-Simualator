using System;
using Autofac;
using Autofac.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinFormsGraphicsDevice
{
    public class XnaWinFormModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContentManager>().WithParameters(
                new Parameter[]
                    {
                        new ResolvedParameter(
                            ( pInfo, context ) => pInfo.ParameterType == typeof ( IServiceProvider ),
                            ( pinfo, context ) => context.Resolve<IServiceProvider>() ),
                        new NamedParameter( "rootDirectory", @"..\Content" )
                    } )
                .SingleInstance();

            builder.RegisterType<GraphicsDevice>().SingleInstance();

            builder.RegisterType<GraphicsDeviceService>().WithParameters(
                new Parameter[]
                    {
                        new NamedPropertyParameter( "width", 800 ),
                        new NamedPropertyParameter( "height", 600 )
                    } )
                .As<GraphicsDeviceService>()
                .As<IGraphicsDeviceService>()
                .SingleInstance().OnActivated( c => Mouse.WindowHandle = c.Instance.WindowHandle );

            base.Load( builder );
        }
    }
}