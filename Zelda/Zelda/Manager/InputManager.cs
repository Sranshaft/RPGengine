using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Zelda.EventHandlers;

namespace Zelda.Manager
{
    class InputManager
    {
        private KeyboardState _keyState;
        private KeyboardState _lastKeyState;
        private Keys _lastKey;
        
        private TimeSpan _previousRefreshTime, _refreshTime;

        private static bool ThrottleInput { get; set; }
        private static bool LockMovement { get; set; }

        private static event EventHandler<InputEventArgs> _eventInput;
        public static event EventHandler<InputEventArgs> EventInput
        {
            add { _eventInput += value; }
            remove { _eventInput -= value; }
        }

        public InputManager()
        {
            ThrottleInput = false;
            LockMovement = false;

            _refreshTime = TimeSpan.FromMilliseconds(20);
            _previousRefreshTime = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - _previousRefreshTime > _refreshTime)
            {
                // Reset counter
                _previousRefreshTime = gameTime.TotalGameTime;

                ComputerControls(gameTime);
            }
        }

        private void ComputerControls(GameTime gameTime)
        {
            _keyState = Keyboard.GetState();

            if (_keyState.IsKeyUp(_lastKey) && _lastKey != Keys.None)
            {
                if (_eventInput != null)
                    _eventInput(this, new InputEventArgs(Input.None));
            }

            _lastKeyState = _keyState;

            CheckKeyState(Keys.Enter, Input.Enter);
            CheckKeyState(Keys.F5, Input.F5);
            CheckKeyState(Keys.A, Input.A);

            // Check direction movement

            if (CheckKeyState(Keys.Left, Input.Left))
                return;

            if (CheckKeyState(Keys.Right, Input.Right))
                return;

            if (CheckKeyState(Keys.Up, Input.Up))
                return;

            if (CheckKeyState(Keys.Down, Input.Down))
                return;
        }

        private bool CheckKeyState(Keys key, Input input)
        {
            if (_keyState.IsKeyDown(key))
            {
                if (!ThrottleInput || (ThrottleInput && _lastKeyState.IsKeyUp(key)))
                {
                    if (_eventInput != null)
                    {
                        _eventInput(this, new InputEventArgs(input));
                        _lastKey = key;

                        return true;
                    }

                }
            }

            return false;
        }
    }
}
