using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Xna;
using XnaVs10;

namespace XnaRoadTrafficConstructor
{
    public class KeyboardInputNotify
    {
        private KeyboardState _oldState = Keyboard.GetState();

        public KeyboardInputNotify()
        {
            this.ObservableKeyPressed = Observable.FromEvent<KeyboardKeysChangedArgs>( t => this.KeyPressed += t, t => this.KeyPressed -= t );
            this.ObservableKeyRelease = Observable.FromEvent<KeyboardKeysChangedArgs>( t => this.KeyRelease += t, t => this.KeyRelease -= t );
        }

        public IObservable<IEvent<KeyboardKeysChangedArgs>> ObservableKeyRelease { get; private set; }

        public IObservable<IEvent<KeyboardKeysChangedArgs>> ObservableKeyPressed { get; private set; }

        public void Update(KeyboardState state)
        {
            var pressedKeys = state.GetPressedKeys().Where(k => _oldState[k] == KeyState.Up);
            var releaseKeys = _oldState.GetPressedKeys().Where(k => state[k] == KeyState.Up);

            foreach (var key in pressedKeys)
            {
                KeyPressed.Raise(this, new KeyboardKeysChangedArgs(key, KeyState.Down));
            }

            foreach (var key in releaseKeys)
            {
                KeyRelease.Raise(this, new KeyboardKeysChangedArgs(key, KeyState.Up));
            }

            _oldState = state;
        }

        public event EventHandler<KeyboardKeysChangedArgs> KeyPressed;

        public event EventHandler<KeyboardKeysChangedArgs> KeyRelease;

        public bool IsKeyPressed( Keys key )
        {
            return _oldState.IsKeyDown( key );
        }
    }
}