

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
        private static readonly TimeSpan deviceLostSleepTime = TimeSpan.FromMilliseconds(50.0);
        private Game game;
        private bool inDeviceTransition;
        private bool isDeviceDirty;
        private bool isFullScreen;
        private bool isReallyFullScreen;
        private ShaderProfile minimumPixelShaderProfile;
        private ShaderProfile minimumVertexShaderProfile = ShaderProfile.VS_1_1;
        private static MultiSampleType[] multiSampleTypes;
        private int resizedBackBufferHeight;
        private int resizedBackBufferWidth;
        private bool synchronizeWithVerticalRetrace = true;
        private bool useResizedBackBuffer;
        public static readonly SurfaceFormat[] ValidAdapterFormats = new SurfaceFormat[] { SurfaceFormat.Bgr32, SurfaceFormat.Bgr555, SurfaceFormat.Bgr565, SurfaceFormat.Bgra1010102 };
        public static readonly SurfaceFormat[] ValidBackBufferFormats = new SurfaceFormat[] { SurfaceFormat.Bgr565, SurfaceFormat.Bgr555, SurfaceFormat.Bgra5551, SurfaceFormat.Bgr32, SurfaceFormat.Color, SurfaceFormat.Bgra1010102 };
        public static readonly DeviceType[] ValidDeviceTypes = new DeviceType[] { DeviceType.Hardware };

        #endregion


        #region Events

        public event EventHandler DeviceCreated;

        public event EventHandler DeviceDisposing;

        public event EventHandler DeviceReset;

        public event EventHandler DeviceResetting;

        public event EventHandler Disposed;

        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        #endregion

        // Methods
        static GraphicsDeviceManager()
        {
            MultiSampleType[] typeArray2 = new MultiSampleType[0x11];
            typeArray2[0] = MultiSampleType.NonMaskable;
            typeArray2[1] = MultiSampleType.SixteenSamples;
            typeArray2[2] = MultiSampleType.FifteenSamples;
            typeArray2[3] = MultiSampleType.FourteenSamples;
            typeArray2[4] = MultiSampleType.ThirteenSamples;
            typeArray2[5] = MultiSampleType.TwelveSamples;
            typeArray2[6] = MultiSampleType.ElevenSamples;
            typeArray2[7] = MultiSampleType.TenSamples;
            typeArray2[8] = MultiSampleType.NineSamples;
            typeArray2[9] = MultiSampleType.EightSamples;
            typeArray2[10] = MultiSampleType.SevenSamples;
            typeArray2[11] = MultiSampleType.SixSamples;
            typeArray2[12] = MultiSampleType.FiveSamples;
            typeArray2[13] = MultiSampleType.FourSamples;
            typeArray2[14] = MultiSampleType.ThreeSamples;
            typeArray2[15] = MultiSampleType.TwoSamples;
            multiSampleTypes = typeArray2;
            depthFormatsWithStencil = new DepthFormat[] { DepthFormat.Depth24Stencil8, DepthFormat.Depth24Stencil4, DepthFormat.Depth24Stencil8Single, DepthFormat.Depth15Stencil1 };
            depthFormatsWithoutStencil = new DepthFormat[] { DepthFormat.Depth24, DepthFormat.Depth32, DepthFormat.Depth16 };
        }

        public GraphicsDeviceManager(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game", Resources.GameCannotBeNull);
            }
            this.game = game;
            if (game.Services.GetService(typeof(IGraphicsDeviceManager)) != null)
            {
                throw new ArgumentException(Resources.GraphicsDeviceManagerAlreadyPresent);
            }
            game.Services.AddService(typeof(IGraphicsDeviceManager), this);
            game.Services.AddService(typeof(IGraphicsDeviceService), this);
            game.Window.SizeChanged += new System.Windows.SizeChangedEventHandler(this.GameWindowClientSizeChanged);
            //game.Window.ScreenDeviceNameChanged += new EventHandler(this.GameWindowScreenDeviceNameChanged);
        }


        private void AddDevices(bool anySuitableDevice, List<GraphicsDeviceInformation> foundDevices)
        {
            IntPtr handle =  new System.Windows.Interop.WindowInteropHelper(this.game.Window).Handle;
            foreach (GraphicsAdapter adapter in GraphicsAdapter.Adapters)
            {
                //if (anySuitableDevice)// || this.IsWindowOnAdapter(handle, adapter))
                {
                    foreach (DeviceType type in ValidDeviceTypes)
                    {
                        try
                        {
                            if (adapter.IsDeviceTypeAvailable(type))
                            {
                                GraphicsDeviceCapabilities capabilities = adapter.GetCapabilities(type);
                                if ((capabilities.DeviceCapabilities.IsDirect3D9Driver && IsValidShaderProfile(capabilities.MaxPixelShaderProfile, this.MinimumPixelShaderProfile)) && IsValidShaderProfile(capabilities.MaxVertexShaderProfile, this.MinimumVertexShaderProfile))
                                {
                                    GraphicsDeviceInformation baseDeviceInfo = new GraphicsDeviceInformation();
                                    baseDeviceInfo.Adapter = adapter;
                                    baseDeviceInfo.DeviceType = type;
                                    baseDeviceInfo.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
                                    baseDeviceInfo.PresentationParameters.EnableAutoDepthStencil = true;
                                    baseDeviceInfo.PresentationParameters.BackBufferCount = 1;
                                    baseDeviceInfo.PresentationParameters.PresentOptions = PresentOptions.None;
                                    baseDeviceInfo.PresentationParameters.SwapEffect = SwapEffect.Discard;
                                    baseDeviceInfo.PresentationParameters.FullScreenRefreshRateInHz = 0;
                                    baseDeviceInfo.PresentationParameters.MultiSampleQuality = 0;
                                    baseDeviceInfo.PresentationParameters.MultiSampleType = MultiSampleType.None;
                                    baseDeviceInfo.PresentationParameters.IsFullScreen = this.IsFullScreen;
                                    baseDeviceInfo.PresentationParameters.PresentationInterval = this.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate;
                                    for (int i = 0; i < ValidAdapterFormats.Length; i++)
                                    {
                                        this.AddDevices(adapter, type, adapter.CurrentDisplayMode, baseDeviceInfo, foundDevices);
                                        if (this.isFullScreen)
                                        {
                                            foreach (DisplayMode mode in adapter.SupportedDisplayModes[ValidAdapterFormats[i]])
                                            {
                                                if ((mode.Width >= 640) && (mode.Height >= 480))
                                                {
                                                    this.AddDevices(adapter, type, mode, baseDeviceInfo, foundDevices);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (DeviceNotSupportedException)
                        {
                        }
                    }
                }
            }
        }

        private void AddDevices(GraphicsAdapter adapter, DeviceType deviceType, DisplayMode mode, GraphicsDeviceInformation baseDeviceInfo, List<GraphicsDeviceInformation> foundDevices)
        {
            for (int i = 0; i < ValidBackBufferFormats.Length; i++)
            {
                SurfaceFormat backBufferFormat = ValidBackBufferFormats[i];
                if (adapter.CheckDeviceType(deviceType, mode.Format, backBufferFormat, this.IsFullScreen))
                {
                    GraphicsDeviceInformation item = baseDeviceInfo.Clone();
                    if (this.IsFullScreen)
                    {
                        item.PresentationParameters.BackBufferWidth = mode.Width;
                        item.PresentationParameters.BackBufferHeight = mode.Height;
                        item.PresentationParameters.FullScreenRefreshRateInHz = mode.RefreshRate;
                    }
                    else if (this.useResizedBackBuffer)
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
                    item.PresentationParameters.AutoDepthStencilFormat = this.ChooseDepthStencilFormat(adapter, deviceType, mode.Format);
                    if (this.PreferMultiSampling)
                    {
                        for (int j = 0; j < multiSampleTypes.Length; j++)
                        {
                            int qualityLevels = 0;
                            MultiSampleType sampleType = multiSampleTypes[j];
                            if (adapter.CheckDeviceMultiSampleType(deviceType, backBufferFormat, this.IsFullScreen, sampleType, out qualityLevels))
                            {
                                GraphicsDeviceInformation information2 = item.Clone();
                                information2.PresentationParameters.MultiSampleType = sampleType;
                                if (!foundDevices.Contains(information2))
                                {
                                    foundDevices.Add(information2);
                                }
                                break;
                            }
                        }
                    }
                    else if (!foundDevices.Contains(item))
                    {
                        foundDevices.Add(item);
                    }
                }
            }
        }

        public void ApplyChanges()
        {
            if ((this.device == null) || this.isDeviceDirty)
            {
                this.ChangeDevice(false);
            }
        }

        protected virtual bool CanResetDevice(GraphicsDeviceInformation newDeviceInfo)
        {
            if (this.device.CreationParameters.DeviceType != newDeviceInfo.DeviceType)
            {
                return false;
            }
            return true;
        }

        private void ChangeDevice(bool forceCreate)
        {
            if (this.game == null)
            {
                throw new InvalidOperationException(Resources.GraphicsComponentNotAttachedToGame);
            }
            this.CheckForAvailableSupportedHardware();
            this.inDeviceTransition = true;
            string screenDeviceName = this.game.Window.Title;
            int width = (int)this.game.Window.ActualWidth;
            int height = (int)this.game.Window.ActualHeight;
            bool flag = false;
            try
            {
                GraphicsDeviceInformation graphicsDeviceInformation = this.FindBestDevice(forceCreate);
                //this.game.Window.BeginScreenDeviceChange(graphicsDeviceInformation.PresentationParameters.IsFullScreen);
                flag = true;
                bool flag2 = true;
                if (!forceCreate && (this.device != null))
                {
                    this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(graphicsDeviceInformation));
                    if (this.CanResetDevice(graphicsDeviceInformation))
                    {
                        try
                        {
                            GraphicsDeviceInformation information2 = graphicsDeviceInformation.Clone();
                            this.MassagePresentParameters(graphicsDeviceInformation.PresentationParameters);
                            this.ValidateGraphicsDeviceInformation(graphicsDeviceInformation);
                            
                            this.device.Reset(information2.PresentationParameters, information2.Adapter);
                            flag2 = false;
                        }
                        catch
                        {
                        }
                    }
                }
                if (flag2)
                {
                    this.CreateDevice(graphicsDeviceInformation);
                }
                PresentationParameters presentationParameters = this.device.PresentationParameters;
                screenDeviceName = this.device.CreationParameters.Adapter.DeviceName;
                this.isReallyFullScreen = presentationParameters.IsFullScreen;
                if (presentationParameters.BackBufferWidth != 0)
                {
                    width = presentationParameters.BackBufferWidth;
                }
                if (presentationParameters.BackBufferHeight != 0)
                {
                    height = presentationParameters.BackBufferHeight;
                }
                this.isDeviceDirty = false;
            }
            finally
            {
                if (flag)
                {
                    //this.game.Window.EndScreenDeviceChange(screenDeviceName, width, height);
                }
                this.inDeviceTransition = false;
            }
        }

        private void CheckForAvailableSupportedHardware()
        {
            bool flag = false;
            bool flag2 = false;
            foreach (GraphicsAdapter adapter in GraphicsAdapter.Adapters)
            {
                if (adapter.IsDeviceTypeAvailable(DeviceType.Hardware))
                {
                    flag = true;
                    GraphicsDeviceCapabilities capabilities = adapter.GetCapabilities(DeviceType.Hardware);
                    if (((capabilities.MaxPixelShaderProfile != ShaderProfile.Unknown) && (capabilities.MaxPixelShaderProfile >= ShaderProfile.PS_1_1)) && capabilities.DeviceCapabilities.IsDirect3D9Driver)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                if (GetSystemMetrics(0x1000) != 0)
                {
                    throw this.CreateNoSuitableGraphicsDeviceException(Resources.NoDirect3DAccelerationRemoteDesktop, null);
                }
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.NoDirect3DAcceleration, null);
            }
            if (!flag2)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.NoPixelShader11OrDDI9Support, null);
            }
        }

        private DepthFormat ChooseDepthStencilFormat(GraphicsAdapter adapter, DeviceType deviceType, SurfaceFormat adapterFormat)
        {
            if (adapter.CheckDeviceFormat(deviceType, adapterFormat, TextureUsage.None, QueryUsages.None, ResourceType.DepthStencilBuffer, this.PreferredDepthStencilFormat))
            {
                return this.PreferredDepthStencilFormat;
            }
            if (Array.IndexOf<DepthFormat>(depthFormatsWithStencil, this.PreferredDepthStencilFormat) >= 0)
            {
                DepthFormat format = this.ChooseDepthStencilFormatFromList(depthFormatsWithStencil, adapter, deviceType, adapterFormat);
                if (format != DepthFormat.Unknown)
                {
                    return format;
                }
            }
            DepthFormat format2 = this.ChooseDepthStencilFormatFromList(depthFormatsWithoutStencil, adapter, deviceType, adapterFormat);
            if (format2 != DepthFormat.Unknown)
            {
                return format2;
            }
            return DepthFormat.Depth24;
        }

        private DepthFormat ChooseDepthStencilFormatFromList(DepthFormat[] availableFormats, GraphicsAdapter adapter, DeviceType deviceType, SurfaceFormat adapterFormat)
        {
            for (int i = 0; i < availableFormats.Length; i++)
            {
                if ((availableFormats[i] != this.PreferredDepthStencilFormat) && adapter.CheckDeviceFormat(deviceType, adapterFormat, TextureUsage.None, QueryUsages.None, ResourceType.DepthStencilBuffer, availableFormats[i]))
                {
                    return availableFormats[i];
                }
            }
            return DepthFormat.Unknown;
        }

        private void CreateDevice(GraphicsDeviceInformation newInfo)
        {
            if (this.device != null)
            {
                this.device.Dispose();
                this.device = null;
            }
            this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(newInfo));
            this.MassagePresentParameters(newInfo.PresentationParameters);
            try
            {
                this.ValidateGraphicsDeviceInformation(newInfo);
                GraphicsDevice device = new GraphicsDevice(newInfo.Adapter, newInfo.DeviceType, new System.Windows.Interop.WindowInteropHelper(this.game.Window).Handle, newInfo.PresentationParameters);
                this.device = device;
                this.device.DeviceResetting += new EventHandler(this.HandleDeviceResetting);
                this.device.DeviceReset += new EventHandler(this.HandleDeviceReset);
                this.device.DeviceLost += new EventHandler(this.HandleDeviceLost);
                this.device.Disposing += new EventHandler(this.HandleDisposing);
            }
            catch (DeviceNotSupportedException exception)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.Direct3DNotAvailable, exception);
            }
            catch (DriverInternalErrorException exception2)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.Direct3DInternalDriverError, exception2);
            }
            catch (ArgumentException exception3)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.Direct3DInvalidCreateParameters, exception3);
            }
            catch (Exception exception4)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.Direct3DCreateError, exception4);
            }
            this.OnDeviceCreated(this, EventArgs.Empty);
        }

        private Exception CreateNoSuitableGraphicsDeviceException(string message, Exception innerException)
        {
            Exception exception = new NoSuitableGraphicsDeviceException(message, innerException);
            exception.Data.Add("MinimumPixelShaderProfile", this.minimumPixelShaderProfile);
            exception.Data.Add("MinimumVertexShaderProfile", this.minimumVertexShaderProfile);
            return exception;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.game != null)
                {
                    if (this.game.Services.GetService(typeof(IGraphicsDeviceService)) == this)
                    {
                        this.game.Services.RemoveService(typeof(IGraphicsDeviceService));
                    }
                    this.game.Window.SizeChanged -= new System.Windows.SizeChangedEventHandler(this.GameWindowClientSizeChanged);
                   // this.game.Window.ScreenDeviceNameChanged -= new EventHandler(this.GameWindowScreenDeviceNameChanged);
                }
                if (this.device != null)
                {
                    this.device.Dispose();
                    this.device = null;
                }
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }

        private bool EnsureDevice()
        {
            if (this.device == null)
            {
                return false;
            }
            return this.EnsureDevicePlatform();
        }

        private bool EnsureDevicePlatform()
        {
            if (this.isReallyFullScreen && !this.game.IsActive)
            {
                return false;
            }
            switch (this.device.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    Thread.Sleep((int)deviceLostSleepTime.TotalMilliseconds);
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    Thread.Sleep((int)deviceLostSleepTime.TotalMilliseconds);
                    try
                    {
                        this.ChangeDevice(false);
                    }
                    catch (DeviceLostException)
                    {
                        return false;
                    }
                    catch
                    {
                        this.ChangeDevice(true);
                    }
                    break;
            }
            return true;
        }

        protected virtual GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice)
        {
            return this.FindBestPlatformDevice(anySuitableDevice);
        }

        private GraphicsDeviceInformation FindBestPlatformDevice(bool anySuitableDevice)
        {
            List<GraphicsDeviceInformation> foundDevices = new List<GraphicsDeviceInformation>();
            this.AddDevices(anySuitableDevice, foundDevices);
            if ((foundDevices.Count == 0) && this.PreferMultiSampling)
            {
                this.PreferMultiSampling = false;
                this.AddDevices(anySuitableDevice, foundDevices);
            }
            if (foundDevices.Count == 0)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.NoCompatibleDevices, null);
            }
            this.RankDevices(foundDevices);
            if (foundDevices.Count == 0)
            {
                throw this.CreateNoSuitableGraphicsDeviceException(Resources.NoCompatibleDevicesAfterRanking, null);
            }
            return foundDevices[0];
        }

        void GameWindowClientSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (game.IsActive)
            {
                if (!this.inDeviceTransition && ((this.game.Window.ActualHeight != 0) || (this.game.Window.ActualWidth != 0)))
                {
                    this.resizedBackBufferWidth = (int)this.game.Window.ActualWidth;
                    this.resizedBackBufferHeight = (int)this.game.Window.ActualHeight;
                    this.useResizedBackBuffer = true;
                    this.ChangeDevice(false);
                }
            }
        }

        private void GameWindowScreenDeviceNameChanged(object sender, EventArgs e)
        {
            if (!this.inDeviceTransition)
            {
                this.ChangeDevice(false);
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(uint smIndex);
        private void HandleDeviceLost(object sender, EventArgs e)
        {
        }

        private void HandleDeviceReset(object sender, EventArgs e)
        {
            this.OnDeviceReset(this, EventArgs.Empty);
        }

        private void HandleDeviceResetting(object sender, EventArgs e)
        {
            this.OnDeviceResetting(this, EventArgs.Empty);
        }

        private void HandleDisposing(object sender, EventArgs e)
        {
            this.OnDeviceDisposing(this, EventArgs.Empty);
        }

        private static bool IsValidShaderProfile(ShaderProfile capsShaderProfile, ShaderProfile minimumShaderProfile)
        {
            if ((capsShaderProfile == ShaderProfile.PS_2_B) && (minimumShaderProfile == ShaderProfile.PS_2_A))
            {
                return false;
            }
            return (capsShaderProfile >= minimumShaderProfile);
        }

  

        private void MassagePresentParameters(PresentationParameters pp)
        {
            bool flag = pp.BackBufferWidth == 0;
            bool flag2 = pp.BackBufferHeight == 0;
            if (!pp.IsFullScreen)
            {
                NativeMethods.RECT rect;
                IntPtr deviceWindowHandle = pp.DeviceWindowHandle;
                if (deviceWindowHandle == IntPtr.Zero)
                {
                    if (this.game == null)
                    {
                        throw new InvalidOperationException(Resources.GraphicsComponentNotAttachedToGame);
                    }
                    deviceWindowHandle = new System.Windows.Interop.WindowInteropHelper(this.game.Window).Handle;
                }
                NativeMethods.GetClientRect(deviceWindowHandle, out rect);
                if (flag && (rect.Right == 0))
                {
                    pp.BackBufferWidth = 1;
                }
                if (flag2 && (rect.Bottom == 0))
                {
                    pp.BackBufferHeight = 1;
                }
            }
        }

        bool IGraphicsDeviceManager.BeginDraw()
        {
            if (!this.EnsureDevice())
            {
                return false;
            }
            this.beginDrawOk = true;
            return true;
        }

        void IGraphicsDeviceManager.CreateDevice()
        {
            this.ChangeDevice(true);
        }

        void IGraphicsDeviceManager.EndDraw()
        {
            if (this.beginDrawOk && (this.device != null))
            {
                try
                {
                    this.device.Present();
                }
                catch (InvalidOperationException)
                {
                }
                catch (DeviceLostException)
                {
                }
                catch (DeviceNotResetException)
                {
                }
                catch (DriverInternalErrorException)
                {
                }
            }
        }

        protected virtual void OnDeviceCreated(object sender, EventArgs args)
        {
            if (this.DeviceCreated != null)
            {
                this.DeviceCreated(sender, args);
            }
        }

        protected virtual void OnDeviceDisposing(object sender, EventArgs args)
        {
            if (this.DeviceDisposing != null)
            {
                this.DeviceDisposing(sender, args);
            }
        }

        protected virtual void OnDeviceReset(object sender, EventArgs args)
        {
            if (this.DeviceReset != null)
            {
                this.DeviceReset(sender, args);
            }
        }

        protected virtual void OnDeviceResetting(object sender, EventArgs args)
        {
            if (this.DeviceResetting != null)
            {
                this.DeviceResetting(sender, args);
            }
        }

        protected virtual void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args)
        {
            if (this.PreparingDeviceSettings != null)
            {
                this.PreparingDeviceSettings(sender, args);
            }
        }

        protected virtual void RankDevices(List<GraphicsDeviceInformation> foundDevices)
        {
            this.RankDevicesPlatform(foundDevices);
        }

        private void RankDevicesPlatform(List<GraphicsDeviceInformation> foundDevices)
        {
            int index = 0;
            while (index < foundDevices.Count)
            {
                DeviceType deviceType = foundDevices[index].DeviceType;
                GraphicsAdapter adapter = foundDevices[index].Adapter;
                PresentationParameters presentationParameters = foundDevices[index].PresentationParameters;
                if (!adapter.CheckDeviceFormat(deviceType, adapter.CurrentDisplayMode.Format, TextureUsage.None, QueryUsages.PostPixelShaderBlending, ResourceType.Texture2D, presentationParameters.BackBufferFormat))
                {
                    foundDevices.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            foundDevices.Sort(new GraphicsDeviceInformationComparer(this));
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
            this.ChangeDevice(false);
        }

        private void ValidateGraphicsDeviceInformation(GraphicsDeviceInformation devInfo)
        {
            SurfaceFormat format;
            GraphicsAdapter adapter = devInfo.Adapter;
            DeviceType deviceType = devInfo.DeviceType;
            bool enableAutoDepthStencil = devInfo.PresentationParameters.EnableAutoDepthStencil;
            DepthFormat autoDepthStencilFormat = devInfo.PresentationParameters.AutoDepthStencilFormat;
            SurfaceFormat backBufferFormat = devInfo.PresentationParameters.BackBufferFormat;
            int backBufferWidth = devInfo.PresentationParameters.BackBufferWidth;
            int backBufferHeight = devInfo.PresentationParameters.BackBufferHeight;
            PresentationParameters presentationParameters = devInfo.PresentationParameters;
            SurfaceFormat format4 = presentationParameters.BackBufferFormat;
            if (!presentationParameters.IsFullScreen)
            {
                format = adapter.CurrentDisplayMode.Format;
                if (SurfaceFormat.Unknown == presentationParameters.BackBufferFormat)
                {
                    format4 = format;
                }
            }
            else
            {
                SurfaceFormat format5 = presentationParameters.BackBufferFormat;
                if (format5 != SurfaceFormat.Color)
                {
                    if (format5 != SurfaceFormat.Bgra5551)
                    {
                        format = presentationParameters.BackBufferFormat;
                    }
                    else
                    {
                        format = SurfaceFormat.Bgr555;
                    }
                }
                else
                {
                    format = SurfaceFormat.Bgr32;
                }
            }
            if (-1 == Array.IndexOf<SurfaceFormat>(ValidBackBufferFormats, format4))
            {
                throw new ArgumentException(Resources.ValidateBackBufferFormatIsInvalid);
            }
            if (!adapter.CheckDeviceType(deviceType, format, presentationParameters.BackBufferFormat, presentationParameters.IsFullScreen))
            {
                throw new ArgumentException(Resources.ValidateDeviceType);
            }
            if ((presentationParameters.BackBufferCount < 0) || (presentationParameters.BackBufferCount > 3))
            {
                throw new ArgumentException(Resources.ValidateBackBufferCount);
            }
            if ((presentationParameters.BackBufferCount > 1) && (presentationParameters.SwapEffect == SwapEffect.Copy))
            {
                throw new ArgumentException(Resources.ValidateBackBufferCountSwapCopy);
            }
            switch (presentationParameters.SwapEffect)
            {
                case SwapEffect.Discard:
                case SwapEffect.Flip:
                case SwapEffect.Copy:
                    {
                        int num3;
                        if (!adapter.CheckDeviceMultiSampleType(deviceType, format4, presentationParameters.IsFullScreen, presentationParameters.MultiSampleType, out num3))
                        {
                            throw new ArgumentException(Resources.ValidateMultiSampleTypeInvalid);
                        }
                        if (presentationParameters.MultiSampleQuality >= num3)
                        {
                            throw new ArgumentException(Resources.ValidateMultiSampleQualityInvalid);
                        }
                        if ((presentationParameters.MultiSampleType != MultiSampleType.None) && (presentationParameters.SwapEffect != SwapEffect.Discard))
                        {
                            throw new ArgumentException(Resources.ValidateMultiSampleSwapEffect);
                        }
                        if (((presentationParameters.PresentOptions & PresentOptions.DiscardDepthStencil) != PresentOptions.None) && !presentationParameters.EnableAutoDepthStencil)
                        {
                            throw new ArgumentException(Resources.ValidateAutoDepthStencilMismatch);
                        }
                        if (presentationParameters.EnableAutoDepthStencil)
                        {
                            if (!adapter.CheckDeviceFormat(deviceType, format, TextureUsage.None, QueryUsages.None, ResourceType.DepthStencilBuffer, presentationParameters.AutoDepthStencilFormat))
                            {
                                throw new ArgumentException(Resources.ValidateAutoDepthStencilFormatInvalid);
                            }
                            if (!adapter.CheckDepthStencilMatch(deviceType, format, format4, presentationParameters.AutoDepthStencilFormat))
                            {
                                throw new ArgumentException(Resources.ValidateAutoDepthStencilFormatIncompatible);
                            }
                        }
                        if (!presentationParameters.IsFullScreen)
                        {
                            if (presentationParameters.FullScreenRefreshRateInHz != 0)
                            {
                                throw new ArgumentException(Resources.ValidateRefreshRateInWindow);
                            }
                            switch (presentationParameters.PresentationInterval)
                            {
                                case PresentInterval.Default:
                                case PresentInterval.One:
                                case PresentInterval.Immediate:
                                    return;
                            }
                            throw new ArgumentException(Resources.ValidatePresentationIntervalInWindow);
                        }
                        if (presentationParameters.FullScreenRefreshRateInHz == 0)
                        {
                            throw new ArgumentException(Resources.ValidateRefreshRateInFullScreen);
                        }
                        GraphicsDeviceCapabilities capabilities = adapter.GetCapabilities(deviceType);
                        switch (presentationParameters.PresentationInterval)
                        {
                            case PresentInterval.Default:
                            case PresentInterval.One:
                            case PresentInterval.Immediate:
                                goto Label_02E5;

                            case PresentInterval.Two:
                            case PresentInterval.Three:
                            case PresentInterval.Four:
                                if ((capabilities.PresentInterval & presentationParameters.PresentationInterval) == PresentInterval.Default)
                                {
                                    throw new ArgumentException(Resources.ValidatePresentationIntervalIncompatibleInFullScreen);
                                }
                                goto Label_02E5;
                        }
                        break;
                    }
                default:
                    throw new ArgumentException(Resources.ValidateSwapEffectInvalid);
            }
            throw new ArgumentException(Resources.ValidatePresentationIntervalInFullScreen);
        Label_02E5:
            if (presentationParameters.IsFullScreen)
            {
                if ((presentationParameters.BackBufferWidth == 0) || (presentationParameters.BackBufferHeight == 0))
                {
                    throw new ArgumentException(Resources.ValidateBackBufferDimsFullScreen);
                }
                bool flag2 = true;
                bool flag3 = false;
                DisplayMode currentDisplayMode = adapter.CurrentDisplayMode;
                if (((currentDisplayMode.Format != format) && (currentDisplayMode.Width != presentationParameters.BackBufferHeight)) && ((currentDisplayMode.Height != presentationParameters.BackBufferHeight) && (currentDisplayMode.RefreshRate != presentationParameters.FullScreenRefreshRateInHz)))
                {
                    flag2 = false;
                    foreach (DisplayMode mode2 in adapter.SupportedDisplayModes[format])
                    {
                        if ((mode2.Width == presentationParameters.BackBufferWidth) && (mode2.Height == presentationParameters.BackBufferHeight))
                        {
                            flag3 = true;
                            if (mode2.RefreshRate == presentationParameters.FullScreenRefreshRateInHz)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                }
                if (!flag2 && flag3)
                {
                    throw new ArgumentException(Resources.ValidateBackBufferDimsModeFullScreen);
                }
                if (!flag2)
                {
                    throw new ArgumentException(Resources.ValidateBackBufferHzModeFullScreen);
                }
            }
            if (presentationParameters.EnableAutoDepthStencil != enableAutoDepthStencil)
            {
                throw new ArgumentException(Resources.ValidateAutoDepthStencilAdapterGroup);
            }
            if (presentationParameters.EnableAutoDepthStencil)
            {
                if (presentationParameters.AutoDepthStencilFormat != autoDepthStencilFormat)
                {
                    throw new ArgumentException(Resources.ValidateAutoDepthStencilAdapterGroup);
                }
                if (presentationParameters.BackBufferFormat != backBufferFormat)
                {
                    throw new ArgumentException(Resources.ValidateAutoDepthStencilAdapterGroup);
                }
                if (presentationParameters.BackBufferWidth != backBufferWidth)
                {
                    throw new ArgumentException(Resources.ValidateAutoDepthStencilAdapterGroup);
                }
                if (presentationParameters.BackBufferHeight != backBufferHeight)
                {
                    throw new ArgumentException(Resources.ValidateAutoDepthStencilAdapterGroup);
                }
            }
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

        public ShaderProfile MinimumPixelShaderProfile
        {
            get
            {
                return this.minimumPixelShaderProfile;
            }
            set
            {
                if ((value < ShaderProfile.PS_1_1) || (value > ShaderProfile.XPS_3_0))
                {
                    throw new ArgumentOutOfRangeException("value", Resources.InvalidPixelShaderProfile);
                }
                this.minimumPixelShaderProfile = value;
                this.isDeviceDirty = true;
            }
        }

        public ShaderProfile MinimumVertexShaderProfile
        {
            get
            {
                return this.minimumVertexShaderProfile;
            }
            set
            {
                if ((value < ShaderProfile.VS_1_1) || (value > ShaderProfile.XVS_3_0))
                {
                    throw new ArgumentOutOfRangeException("value", Resources.InvalidVertexShaderProfile);
                }
                this.minimumVertexShaderProfile = value;
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
                if (Array.IndexOf<SurfaceFormat>(ValidBackBufferFormats, value) == -1)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.ValidateBackBufferFormatIsInvalid);
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
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
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
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.BackBufferDimMustBePositive);
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
                switch (value)
                {
                    case DepthFormat.Depth24Stencil8:
                    case DepthFormat.Depth24Stencil8Single:
                    case DepthFormat.Depth24Stencil4:
                    case DepthFormat.Depth24:
                    case DepthFormat.Depth32:
                    case DepthFormat.Depth16:
                    case DepthFormat.Depth15Stencil1:
                        this.depthStencilFormat = value;
                        this.isDeviceDirty = true;
                        return;
                }
                throw new ArgumentOutOfRangeException("value", Resources.ValidateDepthStencilFormatIsInvalid);
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
