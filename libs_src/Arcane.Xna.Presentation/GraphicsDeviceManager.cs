

using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Arcane.Xna.Presentation.Properties;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Arcane.Xna.Presentation
{
    public class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {
        #region Fields

        private bool allowMultiSampling;
        private SurfaceFormat backBufferFormat = SurfaceFormat.Color;
        private int backBufferHeight = DefaultBackBufferHeight;
        private int backBufferWidth = DefaultBackBufferWidth;
        private bool beginDrawOk;
        public static readonly int DefaultBackBufferHeight = 600;
        public static readonly int DefaultBackBufferWidth = 800;
        private static DepthFormat[] depthFormatsWithoutStencil;
        private static DepthFormat[] depthFormatsWithStencil;
        private DepthFormat depthStencilFormat = DepthFormat.Depth24;
        private GraphicsDevice device;
        private static readonly TimeSpan deviceLostSleepTime = TimeSpan.FromMilliseconds( 50.0 );
        private Game game;
        private bool inDeviceTransition;
        private bool isDeviceDirty;
        private bool isFullScreen;
        private bool isReallyFullScreen;
        private int resizedBackBufferHeight;
        private int resizedBackBufferWidth;
        private bool synchronizeWithVerticalRetrace = true;
        private bool useResizedBackBuffer;
        public static readonly SurfaceFormat[] ValidAdapterFormats = new[]
                                                                         {
                                                                             SurfaceFormat.Bgra4444,
                                                                             SurfaceFormat.Bgr565,
                                                                             SurfaceFormat.Bgra5551
                                                                         };
        public static readonly SurfaceFormat[] ValidBackBufferFormats = new[]
                                                                            {
                                                                                SurfaceFormat.Bgr565,
                                                                                SurfaceFormat.Bgra5551,
                                                                                SurfaceFormat.Color,
                                                                                SurfaceFormat.Bgra4444
                                                                            };

        #endregion


        #region Events

        public event EventHandler<EventArgs> DeviceCreated;

        public event EventHandler<EventArgs> DeviceDisposing;

        public event EventHandler<EventArgs> DeviceReset;

        public event EventHandler<EventArgs> DeviceResetting;

        public event EventHandler Disposed;

        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        #endregion

        // Methods
        static GraphicsDeviceManager()
        {
            depthFormatsWithStencil = new DepthFormat[]
                                          {
                                              DepthFormat.Depth24Stencil8, 
                                              DepthFormat.None,
                                              DepthFormat.Depth16,
                                              DepthFormat.Depth24
                                          };
            depthFormatsWithoutStencil = new DepthFormat[]
                                             {
                                                 DepthFormat.Depth24,
                                                 DepthFormat.Depth24Stencil8,
                                                 DepthFormat.Depth16
                                             };
        }

        public GraphicsDeviceManager( Game game )
        {
            if ( game == null )
            {
                throw new ArgumentNullException( "game", Resources.GameCannotBeNull );
            }
            this.game = game;
            game.Window.SizeChanged += new System.Windows.SizeChangedEventHandler( this.GameWindowClientSizeChanged );
        }


        private void AddDevices( bool anySuitableDevice, List<GraphicsDeviceInformation> foundDevices )
        {
            IntPtr handle = new System.Windows.Interop.WindowInteropHelper( this.game.Window ).Handle;
            foreach ( GraphicsAdapter adapter in GraphicsAdapter.Adapters )
            {
                //if (anySuitableDevice)// || this.IsWindowOnAdapter(handle, adapter))
                {
                    GraphicsDeviceInformation baseDeviceInfo = new GraphicsDeviceInformation();
                    baseDeviceInfo.Adapter = adapter;
                    baseDeviceInfo.GraphicsProfile = GraphicsProfile.Reach;
                    //                    baseDeviceInfo.DeviceType = type;
                    baseDeviceInfo.PresentationParameters.BackBufferFormat = SurfaceFormat.Color;
                    baseDeviceInfo.PresentationParameters.DepthStencilFormat = DepthFormat.None;
                    baseDeviceInfo.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
                    baseDeviceInfo.PresentationParameters.IsFullScreen = this.IsFullScreen;
                    baseDeviceInfo.PresentationParameters.PresentationInterval = this.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate;
                    for ( int i = 0; i < ValidAdapterFormats.Length; i++ )
                    {
                        this.AddDevices( adapter, adapter.CurrentDisplayMode, baseDeviceInfo, foundDevices );
                        if ( this.isFullScreen )
                        {
                            foreach ( DisplayMode mode in adapter.SupportedDisplayModes[ ValidAdapterFormats[ i ] ] )
                            {
                                if ( ( mode.Width >= 640 ) && ( mode.Height >= 480 ) )
                                {
                                    this.AddDevices( adapter, mode, baseDeviceInfo, foundDevices );
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddDevices( GraphicsAdapter adapter, DisplayMode mode, GraphicsDeviceInformation baseDeviceInfo, List<GraphicsDeviceInformation> foundDevices )
        {
            for ( int i = 0; i < ValidBackBufferFormats.Length; i++ )
            {
                SurfaceFormat backBufferFormat = ValidBackBufferFormats[ i ];
                GraphicsDeviceInformation item = baseDeviceInfo.Clone();
                if ( this.IsFullScreen )
                {
                    item.PresentationParameters.BackBufferWidth = mode.Width;
                    item.PresentationParameters.BackBufferHeight = mode.Height;
                    //                    item.PresentationParameters.FullScreenRefreshRateInHz = mode.RefreshRate;
                }
                else if ( this.useResizedBackBuffer )
                {
                    item.PresentationParameters.BackBufferWidth = this.resizedBackBufferWidth;
                    item.PresentationParameters.BackBufferHeight = this.resizedBackBufferHeight;
                }
                else
                {
                    item.PresentationParameters.BackBufferWidth = this.PreferredBackBufferWidth;
                    item.PresentationParameters.BackBufferHeight = this.PreferredBackBufferHeight;
                }
                item.PresentationParameters.BackBufferFormat = backBufferFormat;
                //                item.PresentationParameters.AutoDepthStencilFormat = this.ChooseDepthStencilFormat();
                item.PresentationParameters.DepthStencilFormat = this.ChooseDepthStencilFormat();
                foundDevices.Add( item );
            }
        }

        public void ApplyChanges()
        {
            if ( ( this.device == null ) || this.isDeviceDirty )
            {
                this.ChangeDevice( false );
            }
        }

        protected virtual bool CanResetDevice( GraphicsDeviceInformation newDeviceInfo )
        {
            //TODO Check
            if ( this.device.Adapter.DeviceId != newDeviceInfo.Adapter.DeviceId )
            {
                return false;
            }
            return true;
        }

        private void ChangeDevice( bool forceCreate )
        {
            if ( this.game == null )
            {
                throw new InvalidOperationException( Resources.GraphicsComponentNotAttachedToGame );
            }
            this.inDeviceTransition = true;
            string screenDeviceName = this.game.Window.Title;
            int width = ( int ) this.game.Window.ActualWidth;
            int height = ( int ) this.game.Window.ActualHeight;
            bool flag = false;
            try
            {
                GraphicsDeviceInformation graphicsDeviceInformation = this.FindBestDevice( forceCreate );
                //this.game.Window.BeginScreenDeviceChange(graphicsDeviceInformation.PresentationParameters.IsFullScreen);
                graphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Color;
                graphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.None;

                flag = true;
                bool flag2 = true;
                if ( !forceCreate && ( this.device != null ) )
                {
                    this.OnPreparingDeviceSettings( this, new PreparingDeviceSettingsEventArgs( graphicsDeviceInformation ) );
                    if ( this.CanResetDevice( graphicsDeviceInformation ) )
                    {
                        try
                        {
                            GraphicsDeviceInformation information2 = graphicsDeviceInformation.Clone();
                            this.MassagePresentParameters( graphicsDeviceInformation.PresentationParameters );

                            this.device.Reset( information2.PresentationParameters, information2.Adapter );
                            flag2 = false;
                        }
                        catch
                        {
                        }
                    }
                }
                if ( flag2 )
                {
                    this.CreateDevice( graphicsDeviceInformation );
                }
                PresentationParameters presentationParameters = this.device.PresentationParameters;
                //                screenDeviceName = this.device.CreationParameters.Adapter.DeviceName;
                this.isReallyFullScreen = presentationParameters.IsFullScreen;
                if ( presentationParameters.BackBufferWidth != 0 )
                {
                    width = presentationParameters.BackBufferWidth;
                }
                if ( presentationParameters.BackBufferHeight != 0 )
                {
                    height = presentationParameters.BackBufferHeight;
                }
                this.isDeviceDirty = false;
            }
            finally
            {
                if ( flag )
                {
                    //                    this.game.Window.EndScreenDeviceChange(screenDeviceName, width, height);
                }
                this.inDeviceTransition = false;
            }
        }

        private DepthFormat ChooseDepthStencilFormat()
        {
            // TODO Check
            return DepthFormat.Depth24;
        }

        private void CreateDevice( GraphicsDeviceInformation newInfo )
        {
            if ( this.device != null )
            {
                this.device.Dispose();
                this.device = null;
            }
            this.OnPreparingDeviceSettings( this, new PreparingDeviceSettingsEventArgs( newInfo ) );
            this.MassagePresentParameters( newInfo.PresentationParameters );
            try
            {
                newInfo.PresentationParameters.DeviceWindowHandle = new System.Windows.Interop.WindowInteropHelper( this.game.Window ).Handle;
                var graphicsDevice = new GraphicsDevice( newInfo.Adapter, GraphicsProfile.Reach, newInfo.PresentationParameters );
                this.device = graphicsDevice;
                this.device.DeviceResetting += this.HandleDeviceResetting;
                this.device.DeviceReset += this.HandleDeviceReset;
                this.device.DeviceLost += this.HandleDeviceLost;
                this.device.Disposing += this.HandleDisposing;
            }
            catch ( ArgumentException exception3 )
            {
                throw this.CreateNoSuitableGraphicsDeviceException( Resources.Direct3DInvalidCreateParameters, exception3 );
            }
            catch ( Exception exception4 )
            {
                throw this.CreateNoSuitableGraphicsDeviceException( Resources.Direct3DCreateError, exception4 );
            }
            this.OnDeviceCreated( this, EventArgs.Empty );
        }

        private Exception CreateNoSuitableGraphicsDeviceException( string message, Exception innerException )
        {
            return new NoSuitableGraphicsDeviceException( message, innerException );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( this.game != null )
                {
                    if ( this.game.Services.GetService( typeof( IGraphicsDeviceService ) ) == this )
                    {
//                        this.game.Services.RemoveService( typeof( IGraphicsDeviceService ) );
                    }
                    this.game.Window.SizeChanged -= new System.Windows.SizeChangedEventHandler( this.GameWindowClientSizeChanged );
                    // this.game.Window.ScreenDeviceNameChanged -= new EventHandler(this.GameWindowScreenDeviceNameChanged);
                }
                if ( this.device != null )
                {
                    this.device.Dispose();
                    this.device = null;
                }
                if ( this.Disposed != null )
                {
                    this.Disposed( this, EventArgs.Empty );
                }
            }
        }

        private bool EnsureDevice()
        {
            if ( this.device == null )
            {
                return false;
            }
            return this.EnsureDevicePlatform();
        }

        private bool EnsureDevicePlatform()
        {
            if ( this.isReallyFullScreen && !this.game.IsActive )
            {
                return false;
            }
            switch ( this.device.GraphicsDeviceStatus )
            {
                case GraphicsDeviceStatus.Lost:
                    Thread.Sleep( ( int ) deviceLostSleepTime.TotalMilliseconds );
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    Thread.Sleep( ( int ) deviceLostSleepTime.TotalMilliseconds );
                    try
                    {
                        this.ChangeDevice( false );
                    }
                    catch ( DeviceLostException )
                    {
                        return false;
                    }
                    catch
                    {
                        this.ChangeDevice( true );
                    }
                    break;
            }
            return true;
        }

        protected virtual GraphicsDeviceInformation FindBestDevice( bool anySuitableDevice )
        {
            return this.FindBestPlatformDevice( anySuitableDevice );
        }

        private GraphicsDeviceInformation FindBestPlatformDevice( bool anySuitableDevice )
        {
            List<GraphicsDeviceInformation> foundDevices = new List<GraphicsDeviceInformation>();
            this.AddDevices( anySuitableDevice, foundDevices );
            if ( ( foundDevices.Count == 0 ) && this.PreferMultiSampling )
            {
                this.PreferMultiSampling = false;
                this.AddDevices( anySuitableDevice, foundDevices );
            }
            if ( foundDevices.Count == 0 )
            {
                throw this.CreateNoSuitableGraphicsDeviceException( Resources.NoCompatibleDevices, null );
            }
            if ( foundDevices.Count == 0 )
            {
                throw this.CreateNoSuitableGraphicsDeviceException( Resources.NoCompatibleDevicesAfterRanking, null );
            }
            return foundDevices[ 0 ];
        }

        void GameWindowClientSizeChanged( object sender, System.Windows.SizeChangedEventArgs e )
        {
            if ( game.IsActive )
            {
                if ( !this.inDeviceTransition && ( ( this.game.Window.ActualHeight != 0 ) || ( this.game.Window.ActualWidth != 0 ) ) )
                {
                    this.resizedBackBufferWidth = ( int ) this.game.Window.ActualWidth;
                    this.resizedBackBufferHeight = ( int ) this.game.Window.ActualHeight;
                    this.useResizedBackBuffer = true;
                    this.ChangeDevice( false );
                }
            }
        }

        private void GameWindowScreenDeviceNameChanged( object sender, EventArgs e )
        {
            if ( !this.inDeviceTransition )
            {
                this.ChangeDevice( false );
            }
        }

        [DllImport( "user32.dll" )]
        private static extern int GetSystemMetrics( uint smIndex );
        private void HandleDeviceLost( object sender, EventArgs e )
        {
        }

        private void HandleDeviceReset( object sender, EventArgs e )
        {
            this.OnDeviceReset( this, EventArgs.Empty );
        }

        private void HandleDeviceResetting( object sender, EventArgs e )
        {
            this.OnDeviceResetting( this, EventArgs.Empty );
        }

        private void HandleDisposing( object sender, EventArgs e )
        {
            this.OnDeviceDisposing( this, EventArgs.Empty );
        }


        private void MassagePresentParameters( PresentationParameters pp )
        {
            bool flag = pp.BackBufferWidth == 0;
            bool flag2 = pp.BackBufferHeight == 0;
            if ( !pp.IsFullScreen )
            {
                NativeMethods.RECT rect;
                IntPtr deviceWindowHandle = pp.DeviceWindowHandle;
                if ( deviceWindowHandle == IntPtr.Zero )
                {
                    if ( this.game == null )
                    {
                        throw new InvalidOperationException( Resources.GraphicsComponentNotAttachedToGame );
                    }
                    deviceWindowHandle = new System.Windows.Interop.WindowInteropHelper( this.game.Window ).Handle;
                }
                NativeMethods.GetClientRect( deviceWindowHandle, out rect );
                if ( flag && ( rect.Right == 0 ) )
                {
                    pp.BackBufferWidth = 1;
                }
                if ( flag2 && ( rect.Bottom == 0 ) )
                {
                    pp.BackBufferHeight = 1;
                }
            }
        }

        bool IGraphicsDeviceManager.BeginDraw()
        {
            if ( !this.EnsureDevice() )
            {
                return false;
            }
            this.beginDrawOk = true;
            return true;
        }

        void IGraphicsDeviceManager.CreateDevice()
        {
            this.ChangeDevice( true );
        }

        void IGraphicsDeviceManager.EndDraw()
        {
            if ( this.beginDrawOk && ( this.device != null ) )
            {
                try
                {
                    this.device.Present();
                }
                catch ( InvalidOperationException )
                {
                }
                catch ( DeviceLostException )
                {
                }
                catch ( DeviceNotResetException )
                {
                }
            }
        }

        protected virtual void OnDeviceCreated( object sender, EventArgs args )
        {
            if ( this.DeviceCreated != null )
            {
                this.DeviceCreated( sender, args );
            }
        }

        protected virtual void OnDeviceDisposing( object sender, EventArgs args )
        {
            if ( this.DeviceDisposing != null )
            {
                this.DeviceDisposing( sender, args );
            }
        }

        protected virtual void OnDeviceReset( object sender, EventArgs args )
        {
            if ( this.DeviceReset != null )
            {
                this.DeviceReset( sender, args );
            }
        }

        protected virtual void OnDeviceResetting( object sender, EventArgs args )
        {
            if ( this.DeviceResetting != null )
            {
                this.DeviceResetting( sender, args );
            }
        }

        protected virtual void OnPreparingDeviceSettings( object sender, PreparingDeviceSettingsEventArgs args )
        {
            if ( this.PreparingDeviceSettings != null )
            {
                this.PreparingDeviceSettings( sender, args );
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
            this.ChangeDevice( false );
        }


        // Properties
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.device;
            }
        }

        public bool IsFullScreen
        {
            get
            {
                return this.isFullScreen;
            }
            set
            {
                this.isFullScreen = value;
                this.isDeviceDirty = true;
            }
        }

        public bool PreferMultiSampling
        {
            get
            {
                return this.allowMultiSampling;
            }
            set
            {
                this.allowMultiSampling = value;
                this.isDeviceDirty = true;
            }
        }

        public SurfaceFormat PreferredBackBufferFormat
        {
            get
            {
                return this.backBufferFormat;
            }
            set
            {
                if ( Array.IndexOf<SurfaceFormat>( ValidBackBufferFormats, value ) == -1 )
                {
                    throw new ArgumentOutOfRangeException( "value", Resources.ValidateBackBufferFormatIsInvalid );
                }
                this.backBufferFormat = value;
                this.isDeviceDirty = true;
            }
        }

        public int PreferredBackBufferHeight
        {
            get
            {
                return this.backBufferHeight;
            }
            set
            {
                if ( value <= 0 )
                {
                    throw new ArgumentOutOfRangeException( "value", Resources.BackBufferDimMustBePositive );
                }
                this.backBufferHeight = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }

        public int PreferredBackBufferWidth
        {
            get
            {
                return this.backBufferWidth;
            }
            set
            {
                if ( value <= 0 )
                {
                    throw new ArgumentOutOfRangeException( "value", Resources.BackBufferDimMustBePositive );
                }
                this.backBufferWidth = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }

        public DepthFormat PreferredDepthStencilFormat
        {
            get
            {
                return this.depthStencilFormat;
            }
            set
            {
                switch ( value )
                {
                    case DepthFormat.Depth24Stencil8:
                    case DepthFormat.Depth24:
                    case DepthFormat.Depth16:
                        this.depthStencilFormat = value;
                        this.isDeviceDirty = true;
                        return;
                }
                throw new ArgumentOutOfRangeException( "value", Resources.ValidateDepthStencilFormatIsInvalid );
            }
        }

        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return this.synchronizeWithVerticalRetrace;
            }
            set
            {
                this.synchronizeWithVerticalRetrace = value;
                this.isDeviceDirty = true;
            }
        }
    }


}
