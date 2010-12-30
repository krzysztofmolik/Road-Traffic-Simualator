using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Utils;
using Xna;
using XnaVs10;
using XnaVs10.Road;
using XnaVs10.Utils;

namespace XnaRoadTrafficConstructor.Road
{
    public class ControlManager : IControlManager
    {
        private readonly Camera3D _camera;
        private readonly MouseInputNotify _mouseNotify;
        private readonly KeyboardInputNotify _keyboardInput;

        private readonly IList<EventHandler<CameraChangedEventArgs>> _cameraChangedInvocaionList;

        private readonly IList<EventHandler<MouseStateEventArgs>> _mouseMoveInvocationLit;
        private readonly IList<EventHandler<MouseStateEventArgs>> _mousePressedInvocationList;
        private readonly IList<EventHandler<MouseStateEventArgs>> _mouseReleasedInvocationList;

        private readonly IList<EventHandler<KeyboardKeysChangedArgs>> _keyPressedInvocationList;
        private readonly IList<EventHandler<KeyboardKeysChangedArgs>> _keyReleasedInvocationList;

        public ControlManager( Camera3D camera, MouseInputNotify mouseNotify, KeyboardInputNotify keyboardInput )
        {
            this._cameraChangedInvocaionList = new List<EventHandler<CameraChangedEventArgs>>();

            this._mouseMoveInvocationLit = new List<EventHandler<MouseStateEventArgs>>();
            this._mousePressedInvocationList = new List<EventHandler<MouseStateEventArgs>>();
            this._mouseReleasedInvocationList = new List<EventHandler<MouseStateEventArgs>>();

            this._keyPressedInvocationList = new List<EventHandler<KeyboardKeysChangedArgs>>();
            this._keyReleasedInvocationList = new List<EventHandler<KeyboardKeysChangedArgs>>();

            this._camera = camera;
            this._keyboardInput = keyboardInput;
            this._mouseNotify = mouseNotify;

            this.SubscribeToEvent();
        }

        public IObservable<IEvent<MouseStateEventArgs>> MousePressedObservable
        {
            get
            {
                return Observable.FromEvent<MouseStateEventArgs>(eh => this.MousePressed += eh, eh => this.MouseReleased -= eh);
            }
        }

        public IObservable<IEvent<MouseStateEventArgs>> MouseReleseddObservable
        {
            get
            {
                return Observable.FromEvent<MouseStateEventArgs>(eh => this.MouseReleased += eh, eh => this.MouseReleased -= eh);
            }
        }

        public IObservable<IEvent<KeyboardKeysChangedArgs>> KeyPressedObservable
        {
            get
            {
                return Observable.FromEvent<KeyboardKeysChangedArgs>(eh => this.KeyPressed += eh, eh => this.KeyPressed -= eh);
            }
        }

        public IObservable<IEvent<KeyboardKeysChangedArgs>> KeyRelesedObservable
        {
            get
            {
                return Observable.FromEvent<KeyboardKeysChangedArgs>(eh => this.KeyReleased += eh, eh => this.KeyReleased -= eh);
            }
        }

        public event EventHandler<CameraChangedEventArgs> CameraChagned
        {
            add
            {
                this._cameraChangedInvocaionList.Add( value );
            }

            remove
            {
                this._cameraChangedInvocaionList.Remove( value );
            }
        }

        public event EventHandler<MouseStateEventArgs> MouseMove
        {
            add
            {
                this._mouseMoveInvocationLit.Add( value );
            }

            remove
            {
                this._mouseMoveInvocationLit.Remove( value );
            }
        }

        public event EventHandler<MouseStateEventArgs> MousePressed
        {
            add { this._mousePressedInvocationList.Add( value ); }
            remove { this._mousePressedInvocationList.Remove( value ); }
        }

        public event EventHandler<MouseStateEventArgs> MouseReleased
        {
            add { this._mouseReleasedInvocationList.Add( value ); }
            remove { this._mouseReleasedInvocationList.Remove( value ); }
        }

        public event EventHandler<KeyboardKeysChangedArgs> KeyPressed
        {
            add { this._keyPressedInvocationList.Add( value ); }
            remove { this._keyReleasedInvocationList.Remove( value ); }
        }

        public event EventHandler<KeyboardKeysChangedArgs> KeyReleased
        {
            add { this._keyReleasedInvocationList.Add( value ); }
            remove { this._keyReleasedInvocationList.Remove( value ); }
        }

        private void SubscribeToEvent()
        {
            this._camera.Changed += this.OnCameraChanged;
            this._mouseNotify.MouseMove += this.OnMouseMove;
            this._mouseNotify.MousePressed += this.OnMousePressed;
            this._mouseNotify.MouseRelease += this.OnMouseRelease;

            this._keyboardInput.KeyPressed += this.OnKeyPresed;
            this._keyboardInput.KeyRelease += this.OnKeyRelease;
        }

        private void OnMouseMove( object sender, MouseStateEventArgs e )
        {
            this._mouseMoveInvocationLit.ForEachUntil( t => t.Raise( sender, e ), t => !e.Handled );
        }

        private void OnMouseRelease( object sender, MouseStateEventArgs e )
        {
            this._mouseReleasedInvocationList.ForEachUntil(t => t.Raise(sender, e), t => !e.Handled);
        }

        private void OnKeyPresed( object sender, KeyboardKeysChangedArgs e )
        {
            this._keyPressedInvocationList.ForEachUntil(t => t.Raise(sender, e), t => !e.Handled);
        }

        private void OnKeyRelease( object sender, KeyboardKeysChangedArgs e )
        {
            this._keyReleasedInvocationList.ForEachUntil(t => t.Raise(sender, e), t => !e.Handled);
        }

        private void OnMousePressed( object sender, MouseStateEventArgs e )
        {
            this._mousePressedInvocationList.ForEachUntil(t => t.Raise(sender, e), t => !e.Handled);
        }

        private void OnCameraChanged( object sender, CameraChangedEventArgs e )
        {
            this._cameraChangedInvocaionList.ForEachUntil(t => t.Raise(sender, e), t => !e.Handled);
        }
    }
}