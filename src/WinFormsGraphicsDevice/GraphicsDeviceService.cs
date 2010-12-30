#region File Description
//-----------------------------------------------------------------------------
// GraphicsDeviceService.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Autofac;
using Microsoft.Xna.Framework.Graphics;
#endregion

// The IGraphicsDeviceService interface requires a DeviceCreated event, but we
// always just create the device inside our constructor, so we have no place to
// raise that event. The C# compiler warns us that the event is never used, but
// we don't care so we just disable this warning.
#pragma warning disable 67

namespace WinFormsGraphicsDevice
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    class GraphicsDeviceService : IGraphicsDeviceService
    {
        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        public GraphicsDeviceService( IContainer container, IntPtr windowHandle, int width, int height )
        {
            this.WindowHandle = windowHandle;
            this.parameters = new PresentationParameters
                                  {
                                      BackBufferWidth = Math.Max( width, 1 ),
                                      BackBufferHeight = Math.Max( height, 1 ),
                                      DeviceWindowHandle = windowHandle, 
                                      DepthStencilFormat = DepthFormat.Depth24Stencil8,BackBufferFormat = SurfaceFormat.Color,
                                      

                                      IsFullScreen = false,
                                  };

            //TODO Do zmiany w pierwszej kolejnosci !!
            this.graphicsDevice = container.Resolve<GraphicsDevice>(
                new TypedParameter( typeof( GraphicsAdapter ), GraphicsAdapter.DefaultAdapter ),
                new TypedParameter( typeof( GraphicsProfile ), GraphicsProfile.Reach ),
                new TypedParameter( typeof( PresentationParameters ), this.parameters ) );
        }

        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice( int width, int height )
        {
            if ( DeviceResetting != null )
                DeviceResetting( this, EventArgs.Empty );

            parameters.BackBufferWidth = width;
            parameters.BackBufferHeight = height;

            graphicsDevice.Reset( parameters );

            if ( DeviceReset != null )
                DeviceReset( this, EventArgs.Empty );
        }

        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        GraphicsDevice graphicsDevice;


        // Store the current device settings.
        PresentationParameters parameters;

        public IntPtr WindowHandle { get; set; }


        // IGraphicsDeviceService events.
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
    }
}
