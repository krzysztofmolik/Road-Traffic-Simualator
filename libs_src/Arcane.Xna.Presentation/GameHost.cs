using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace Arcane.Xna.Presentation
{
    /// <summary>
    /// Logique d'interaction pour GameHost.xaml
    /// </summary>
    public partial class GameHost : Window
    {

        #region Fields

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool doneRun;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Game game;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Game gameControl;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Window _topLevelControl;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal Window _frontWindow;

        #endregion


        #region Properties


        internal object WPFHost
        {
            get
            {
                return this._frontWindow.Content;
            }
            set
            {
                this._frontWindow.Content = value;
            }
        }

        /// <summary>
        /// <para>Gets the top level Window for the current Xna 3D scene hoster.</para>
        /// </summary>
        public Window TopLevelWindow
        {
            get
            {
                if (this._topLevelControl != Window.GetWindow(this.game))
                {
                    this._topLevelControl = Window.GetWindow(this.game);
                    this.Owner = this._topLevelControl;
                    this._topLevelControl.Closing -= new System.ComponentModel.CancelEventHandler(_topLevelControl_Closing);
                    this._topLevelControl.Closing += new System.ComponentModel.CancelEventHandler(_topLevelControl_Closing);
                    this._topLevelControl.Closed -= new EventHandler(_topLevelControl_Closed);
                    this._topLevelControl.Closed += new EventHandler(_topLevelControl_Closed);
                    this._topLevelControl.LocationChanged -= new EventHandler(MainWindow_LocationChanged);
                    this._topLevelControl.LocationChanged += new EventHandler(MainWindow_LocationChanged);
                    this.UpdateBounds();
                }
                return this._topLevelControl;
            }
        }


        public IntPtr Handle
        {
            get
            {
                return new System.Windows.Interop.WindowInteropHelper(this).Handle;
            }
        }

        #endregion


        #region Constructors

        public GameHost(Game canvas)
        {
            this.game = canvas;
            this.HookElementEvents();
            this.LockThreadToProcessor();
            this.gameControl = game;
            this.Width = this.Height = 1;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.None;

            this._frontWindow = new Window();
            this._frontWindow.Width = this._frontWindow.Height = 1;
            this._frontWindow.ResizeMode = ResizeMode.NoResize;
            this._frontWindow.ShowInTaskbar = false;
            this._frontWindow.WindowState = WindowState.Normal;
            this._frontWindow.WindowStyle = WindowStyle.None;
            this._frontWindow.AllowsTransparency = true;
            this._frontWindow.Background = null;
        }

        #endregion


        #region Public methods

        public void UpdateBounds()
        {
            if (this.IsVisible)
            {
                GeneralTransform gt = this.game.TransformToVisual(this.TopLevelWindow);

                this.Width = this._frontWindow.Width = this.game.ActualWidth;
                this.Height = this._frontWindow.Height = this.game.ActualHeight;

                this.Left = this._frontWindow.Left = this.TopLevelWindow.Left + gt.Transform(new Point(0, 0)).X + 8;
                this.Top = this._frontWindow.Top = this.TopLevelWindow.Top + gt.Transform(new Point(0, 0)).Y + 28;
            }
        }

        #endregion


        #region Life methods

        [DllImport("kernel32.dll")]
        private static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetProcessAffinityMask(IntPtr hProcess, out UIntPtr lpProcessAffinityMask, out UIntPtr lpSystemAffinityMask);
        private void LockThreadToProcessor()
        {
            UIntPtr lpProcessAffinityMask = UIntPtr.Zero;
            UIntPtr lpSystemAffinityMask = UIntPtr.Zero;
            if (GetProcessAffinityMask(GetCurrentProcess(), out lpProcessAffinityMask, out lpSystemAffinityMask) && (lpProcessAffinityMask != UIntPtr.Zero))
            {
                UIntPtr dwThreadAffinityMask = (UIntPtr)(lpProcessAffinityMask.ToUInt64() & (~lpProcessAffinityMask.ToUInt64() + 1));
                SetThreadAffinityMask(GetCurrentThread(), dwThreadAffinityMask);
            }
        }

        internal void Exit()
        {
            //this.exitRequested = true;
        }

        internal void PreRun()
        {
            if (this.doneRun)
            {
                throw new InvalidOperationException(Properties.Resources.NoMultipleRuns);
            }
            try
            {
            }
            catch (Exception)
            {
                PostRun();
                throw;
            }
        }

        internal void PostRun()
        {

            this.doneRun = true;

        }

        #endregion


        #region Events

        private void HookElementEvents()
        {
            this.game.SizeChanged += new SizeChangedEventHandler(this.Game_SizeChanged);
            this.game.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.Game_IsVisibleChanged);
        }

        void _topLevelControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner = null;
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            this.UpdateBounds();
        }

        void Game_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBounds();
        }

        void _topLevelControl_Closed(object sender, EventArgs e)
        {
         }

        void Game_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (!this.IsActive)
                {
                    this.Show();
                    if (this._frontWindow.Owner != this)
                        this._frontWindow.Owner = this;
                    this._frontWindow.Show();
                }
             }
            else
            {
                this.Hide();
                this._frontWindow.Hide();
            }
        }

        #endregion

    }
}
