using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace Arcane.Xna.Presentation
{
    /// <summary>
    /// Logique d'interaction pour GameCanvas.xaml
    /// </summary>
    [System.Windows.Markup.ContentProperty("WPFHost")]
    public partial class Game : Canvas, IDisposable
    {

        #region Fields

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GameHost _window;
        private List<IUpdateable> _updateableComponents = new List<IUpdateable>();
        private List<IDrawable> _drawableComponents = new List<IDrawable>();
        private bool _inRun;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private TimeSpan inactiveSleepTime;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DispatcherTimer _tickGenerator;
        private bool isFixedTimeStep = true;
        private bool isMouseVisible;
        private GameClock clock;
        private TimeSpan lastFrameElapsedGameTime;
        private readonly TimeSpan maximumElapsedTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan targetElapsedTime;
        private TimeSpan totalGameTime;
        private TimeSpan accumulatedElapsedGameTime;
        private GameServiceContainer gameServices = new GameServiceContainer();
        private GameTime gameTime = new GameTime();
        private IGraphicsDeviceManager graphicsDeviceManager;
        private IGraphicsDeviceService graphicsDeviceService;
        private bool doneFirstUpdate;
        private bool drawRunningSlowly;
        private TimeSpan elapsedRealTime;
        private bool exitRequested;
        private ContentManager content;
        private bool _isActivated;

        #endregion


        #region Events

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler Disposed;
        public event EventHandler Exiting;

        private new UIElementCollection Children
        {
            get;
            set;
        }

        public object WPFHost
        {
            get
            {
                if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
                    return this.Window.WPFHost;
                else
                    return (base.Children[0] as ContentControl).Content;
            }
            set
            {
                if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
                    this.Window.WPFHost = value;
                else
                    (base.Children[0] as ContentControl).Content = value;
            }
        }

        protected virtual void OnActivated(object sender, EventArgs args)
        {
            if (!this.IsRunning)
                this.Run();
            if (this.Activated != null)
            {
                this.Activated(this, args);
            }
            if (this.IsRunning)
            {
                this._tickGenerator.Start();
            }
        }

        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            if (this.Deactivated != null)
            {
                this.Deactivated(this, args);
            }
            this._tickGenerator.Stop();
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            this.UnhookDeviceEvents();
            if (this.Exiting != null)
            {
                this.Exiting(null, args);
            }
        }

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (this.Disposed != null)
            {
                this.Disposed(null, args);
            }
        }

        #endregion


        #region Properties

        public event EventHandler FirstActivation;

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                IGraphicsDeviceService graphicsDeviceService = this.graphicsDeviceService;
                if (graphicsDeviceService == null)
                {
                    graphicsDeviceService = this.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                    if (graphicsDeviceService == null)
                    {
                        throw new InvalidOperationException(Properties.Resources.NoGraphicsDeviceService);
                    }
                }
                return graphicsDeviceService.GraphicsDevice;
            }
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public ContentManager Content
        {
            get
            {
                return this.content;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.content = value;
            }
        }

        public GameComponentCollection Components
        {
            get;
            private set;
        }

        public TimeSpan InactiveSleepTime
        {
            get
            {
                return this.inactiveSleepTime;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("Resources.InactiveSleepTimeCannotBeZero", "value");
                }
                this.inactiveSleepTime = value;
            }
        }

        public bool IsFixedTimeStep
        {
            get
            {
                return this.isFixedTimeStep;
            }
            set
            {
                if (this.isFixedTimeStep != value)
                {
                    this.isFixedTimeStep = value;
                    this.RecomputeStepSpan();
                }
            }
        }

        public GameServiceContainer Services
        {
            get;
            private set;
        }

        public bool IsActive
        {
            get
            {
                return this._isActivated;
            }
            internal set
            {
                this._isActivated = value;
                if (value)
                    OnActivated(this, EventArgs.Empty);
                else
                    OnDeactivated(this, EventArgs.Empty);

            }
        }

        public GameHost Window
        {
            get
            {
                return this._window;
            }
        }

        public bool IsMouseVisible
        {
            get
            {
                return this.isMouseVisible;
            }
            set
            {
                this.isMouseVisible = value;
                if (value)
                    this.Window.Cursor = System.Windows.Input.Cursors.None;
                else
                    this.Window.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        #endregion


        #region Constructors

        public Game()
        {
            // InitializeComponent();
            if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
            {
                this._window = new GameHost(this);
                this._window.Closed += new EventHandler(_window_Closed);
                this._tickGenerator = new DispatcherTimer();
                this._tickGenerator.Tick += new EventHandler(_tickGenerator_Tick);
                this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(GameCanvas_IsVisibleChanged);

                this.Components = new GameComponentCollection();
                this.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentAdded);
                this.Components.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentRemoved);

                this.Services = new GameServiceContainer();
                this.content = new ContentManager(this.Services);

                this.IsFixedTimeStep = true;

                this.clock = new GameClock();
                this.totalGameTime = TimeSpan.Zero;
                this.accumulatedElapsedGameTime = TimeSpan.Zero;
                this.lastFrameElapsedGameTime = TimeSpan.Zero;
                this.targetElapsedTime = TimeSpan.FromTicks((long)0x28b0a);
                this.inactiveSleepTime = TimeSpan.FromMilliseconds(20);

                this.gameServices.AddService(typeof(IInputPublisherService), new ControlInputPublisher(this));
            }
            else
                base.Children.Add(new ContentControl());
        }

        void _window_Closed(object sender, EventArgs e)
        {
            this.OnExiting(this, EventArgs.Empty);
            this.OnDisposed(this, EventArgs.Empty);
        }



        private void RecomputeStepSpan()
        {
            this._tickGenerator.Interval = new TimeSpan(0, 0, 0, 0, (this.IsFixedTimeStep ? 33 : 1));
        }

        void GameCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsActive = (bool)e.NewValue;
        }

        void _tickGenerator_Tick(object sender, EventArgs e)
        {
            this.Tick();
        }

        public void Run()
        {
            // Upper part of Run() method
            this.graphicsDeviceManager = this.Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
            if (this.graphicsDeviceManager != null)
            {
                this.graphicsDeviceManager.CreateDevice();
            }
            this.Initialize();
            this._inRun = true;
            try
            {
                this.BeginRun();
                this.gameTime = new GameTime(
                  TimeSpan.Zero, TimeSpan.Zero,
                  this.totalGameTime, this.clock.CurrentTime,
                  false
                );
                this.Update(this.gameTime);
                this.doneFirstUpdate = true;
                if (this.Window != null)
                {
                    this.Window.Owner = Application.Current.MainWindow;
                    this.Window.PreRun();
                }
                this.IsRunning = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                this._inRun = false;
                Microsoft.Xna.Framework.Input.Mouse.WindowHandle = this.Window.Handle;
            }
        }


        protected virtual bool BeginDraw()
        {
            if ((this.graphicsDeviceManager != null) && !this.graphicsDeviceManager.BeginDraw())
            {
                return false;
            }
            return true;
        }

        protected virtual void BeginRun()
        {
        }

        private void DrawFrame()
        {
            if ((this.doneFirstUpdate /*&& !this.Window.IsMinimized*/) && this.BeginDraw())
            {
                this.gameTime = new GameTime(
                  this.clock.CurrentTime, this.elapsedRealTime,
                  this.totalGameTime, this.lastFrameElapsedGameTime,
                  this.drawRunningSlowly
                );

                this.Draw(this.gameTime);
                this.EndDraw();
            }
        }

        protected virtual void EndDraw()
        {
            if (this.graphicsDeviceManager != null)
            {
                this.graphicsDeviceManager.EndDraw();
            }
        }

        protected virtual void EndRun()
        {
        }

        public void Tick()
        {
            if (!this.exitRequested)
            {
                if (!this.IsActive)
                {
                    Thread.Sleep((int)this.inactiveSleepTime.TotalMilliseconds);
                }
                else
                    this.clock.Step();
                this.elapsedRealTime = this.clock.ElapsedTime;
                if (this.elapsedRealTime < TimeSpan.Zero)
                {
                    this.elapsedRealTime = TimeSpan.Zero;
                }
                if (this.elapsedRealTime > this.maximumElapsedTime)
                {
                    this.elapsedRealTime = this.maximumElapsedTime;
                }
                this.gameTime = new GameTime(
                  this.clock.CurrentTime, this.elapsedRealTime,
                  this.gameTime.TotalGameTime, this.gameTime.ElapsedGameTime,
                  this.gameTime.IsRunningSlowly
                );
                this.drawRunningSlowly = false;
                if (this.isFixedTimeStep)
                {
                    this.accumulatedElapsedGameTime += this.elapsedRealTime;
                    long num = this.accumulatedElapsedGameTime.Ticks / this.targetElapsedTime.Ticks;
                    this.accumulatedElapsedGameTime = TimeSpan.FromTicks(this.accumulatedElapsedGameTime.Ticks % this.targetElapsedTime.Ticks);
                    this.lastFrameElapsedGameTime = TimeSpan.Zero;
                    TimeSpan targetElapsedTime = this.targetElapsedTime;
                    if (num > 0)
                    {
                        while (num > 1)
                        {
                            this.drawRunningSlowly = true;
                            this.gameTime = new GameTime(
                              this.gameTime.TotalRealTime, this.gameTime.ElapsedRealTime,
                              this.gameTime.TotalGameTime, this.gameTime.ElapsedGameTime,
                              true
                            );
                            num--;
                            try
                            {
                                this.gameTime = new GameTime(
                                  this.gameTime.TotalRealTime, this.gameTime.ElapsedRealTime,
                                  this.totalGameTime, targetElapsedTime,
                                  this.gameTime.IsRunningSlowly
                                );
                                this.Update(this.gameTime);
                                continue;
                            }
                            finally
                            {
                                this.lastFrameElapsedGameTime += targetElapsedTime;
                                this.totalGameTime += targetElapsedTime;
                            }
                        }
                        this.gameTime = new GameTime(
                          this.gameTime.TotalRealTime, this.gameTime.ElapsedRealTime,
                          this.gameTime.TotalGameTime, this.gameTime.ElapsedGameTime,
                          false
                        );
                        try
                        {
                            this.gameTime = new GameTime(
                              this.gameTime.TotalRealTime, this.gameTime.ElapsedRealTime,
                              this.totalGameTime, targetElapsedTime,
                              this.gameTime.IsRunningSlowly
                            );
                            this.Update(this.gameTime);
                        }
                        finally
                        {
                            this.lastFrameElapsedGameTime += targetElapsedTime;
                            this.totalGameTime += targetElapsedTime;
                        }
                    }
                }
                else
                {
                    TimeSpan elapsedRealTime = this.elapsedRealTime;
                    try
                    {
                        this.gameTime = new GameTime(
                          this.gameTime.TotalRealTime, this.gameTime.ElapsedRealTime,
                          this.totalGameTime, this.lastFrameElapsedGameTime = elapsedRealTime,
                          this.gameTime.IsRunningSlowly
                        );
                        this.Update(this.gameTime);
                    }
                    finally
                    {
                        this.totalGameTime += elapsedRealTime;
                    }
                }
                if (!this.exitRequested)
                {
                    this.DrawFrame();
                }
            }
        }

        public void Exit()
        {
            this.OnExiting(this, EventArgs.Empty);
            this.exitRequested = true;
            this.IsRunning = false;
            this._tickGenerator.Stop();
            this.Window.Hide();
            this.Window.Close();
            this.Window.Exit();
            this.Dispose();
        }

        #endregion


        #region Device life


        private void HookDeviceEvents()
        {
            this.graphicsDeviceService = this.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (this.graphicsDeviceService != null)
            {
                this.graphicsDeviceService.DeviceCreated += new EventHandler(this.DeviceCreated);
                this.graphicsDeviceService.DeviceResetting += new EventHandler(this.DeviceResetting);
                this.graphicsDeviceService.DeviceReset += new EventHandler(this.DeviceReset);
                this.graphicsDeviceService.DeviceDisposing += new EventHandler(this.DeviceDisposing);
            }
        }

        private void UnhookDeviceEvents()
        {
            if (this.graphicsDeviceService != null)
            {
                this.graphicsDeviceService.DeviceCreated -= new EventHandler(this.DeviceCreated);
                this.graphicsDeviceService.DeviceResetting -= new EventHandler(this.DeviceResetting);
                this.graphicsDeviceService.DeviceReset -= new EventHandler(this.DeviceReset);
                this.graphicsDeviceService.DeviceDisposing -= new EventHandler(this.DeviceDisposing);
            }
        }

        private void DeviceCreated(object sender, EventArgs e)
        {
            this.LoadContent();
        }

        private void DeviceDisposing(object sender, EventArgs e)
        {
            this.UnloadContent();
        }

        private void DeviceReset(object sender, EventArgs e)
        {
        }

        private void DeviceResetting(object sender, EventArgs e)
        {

        }

        #endregion


        #region Components life

        private void GameComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            if (this._inRun)
            {
                e.GameComponent.Initialize();
            }
            IUpdateable item = e.GameComponent as IUpdateable;
            if (item != null)
            {
                int num = this._updateableComponents.BinarySearch(item, UpdateOrderComparer.Default);
                if (num < 0)
                {
                    this._updateableComponents.Insert(~num, item);
                    item.UpdateOrderChanged += new EventHandler(this.UpdateableUpdateOrderChanged);
                }
            }
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                int num2 = this._drawableComponents.BinarySearch(drawable, DrawOrderComparer.Default);
                if (num2 < 0)
                {
                    this._drawableComponents.Insert(~num2, drawable);
                    drawable.DrawOrderChanged += new EventHandler(this.DrawableDrawOrderChanged);
                }
            }
        }

        private void GameComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            IUpdateable item = e.GameComponent as IUpdateable;
            if (item != null)
            {
                this._updateableComponents.Remove(item);
                item.UpdateOrderChanged -= new EventHandler(this.UpdateableUpdateOrderChanged);
            }
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                this._drawableComponents.Remove(drawable);
                drawable.DrawOrderChanged -= new EventHandler(this.DrawableDrawOrderChanged);
            }
        }

        private void UpdateableUpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable item = sender as IUpdateable;
            this._updateableComponents.Remove(item);
            int num = this._updateableComponents.BinarySearch(item, UpdateOrderComparer.Default);
            if (num < 0)
            {
                this._updateableComponents.Insert(~num, item);
            }
        }

        private void DrawableDrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable item = sender as IDrawable;
            this._drawableComponents.Remove(item);
            int num = this._drawableComponents.BinarySearch(item, DrawOrderComparer.Default);
            if (num < 0)
            {
                this._drawableComponents.Insert(~num, item);
            }
        }

        #endregion


        #region Game Specific Methods


        /// <summary>
        /// <para>Called when graphics resources need to be loaded.  Override this method to load any game-specific graphics resources.</para>
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void Initialize()
        {
            for (int i = 0; i < this.Components.Count; i++)
            {
                this.Components[i].Initialize();
            }
            this.HookDeviceEvents();
            if ((this.graphicsDeviceService != null) && (this.graphicsDeviceService.GraphicsDevice != null))
            {
                this.LoadContent();
            }
        }

        protected virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < this._updateableComponents.Count; i++)
            {
                IUpdateable updateable = this._updateableComponents[i];
                if (updateable.Enabled)
                {
                    updateable.Update(gameTime);
                }
            }
        }

        protected virtual void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this._drawableComponents.Count; i++)
            {
                IDrawable drawable = this._drawableComponents[i];
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }
        }

        #endregion


        #region IDisposable Membres

        public void Dispose()
        {
            lock (this)
            {
                IGameComponent[] array = new IGameComponent[this.Components.Count];
                this.Components.CopyTo(array, 0);
                for (int i = 0; i < array.Length; i++)
                {
                    IDisposable disposable = array[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                IDisposable disposable2 = this.graphicsDeviceManager as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
                this.UnhookDeviceEvents();

                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}
