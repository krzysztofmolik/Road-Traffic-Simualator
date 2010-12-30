using System;
using System.Collections.Generic;
using Xna;

namespace XnaVs10.Road
{
    public interface IControlManager
    {
        event EventHandler<CameraChangedEventArgs> CameraChagned;

        event EventHandler<MouseStateEventArgs> MouseMove;

        event EventHandler<MouseStateEventArgs> MousePressed;

        event EventHandler<MouseStateEventArgs> MouseReleased;

        event EventHandler<KeyboardKeysChangedArgs> KeyPressed;

        event EventHandler<KeyboardKeysChangedArgs> KeyReleased;

        IObservable<IEvent<MouseStateEventArgs>> MousePressedObservable { get; }

        IObservable<IEvent<MouseStateEventArgs>> MouseReleseddObservable { get; }

        IObservable<IEvent<KeyboardKeysChangedArgs>> KeyPressedObservable { get; }

        IObservable<IEvent<KeyboardKeysChangedArgs>> KeyRelesedObservable { get; }
    }
}