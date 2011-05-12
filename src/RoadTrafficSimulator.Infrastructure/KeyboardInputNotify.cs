using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure
{
    public class KeyboardInputNotify
    {
        private readonly ISubject<KeysState> _keyPressed = new Subject<KeysState>();

        private readonly ISubject<KeysState> _keyReleased = new Subject<KeysState>();

        private KeyboardState _oldState = Keyboard.GetState();

        public IObservable<KeysState> KeyRelease
        {
            get { return this._keyPressed; }
        }

        public IObservable<KeysState> KeyPressed
        {
            get { return this._keyReleased; }
        }

        public void Update( KeyboardState state )
        {
            var pressedKeys = state.GetPressedKeys().Where( k => this._oldState[ k ] == KeyState.Up );
            var releaseKeys = this._oldState.GetPressedKeys().Where( k => state[ k ] == KeyState.Up );

            foreach ( var key in pressedKeys )
            {
                this._keyPressed.OnNext( new KeysState( key ) );
            }

            foreach ( var key in releaseKeys )
            {
                this._keyReleased.OnNext( new KeysState( key ) );
            }

            this._oldState = state;
        }

        public bool IsKeyPressed( Keys key )
        {
            return this._oldState.IsKeyDown( key );
        }
    }
}