using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Manager
{
    public class CameraManager
    {
        private Vector2 _position, _movePosition;
        private float _speed;

        public bool Locked { get { return (int)_position.X != (int)_movePosition.X || (int)_position.Y != (int)_movePosition.Y; } }

        public CameraManager()
        {
            _speed = 16f;
            _position = Vector2.Zero;
        }
        
        public void Update(GameTime gameTime)
        {
            if (!this.Locked)
                return;

            if (_position.X < _movePosition.X)
                _position.X += _speed;

            if (_position.X > _movePosition.X)
                _position.X -= _speed;

            if (_position.Y < _movePosition.Y)
                _position.Y += _speed;

            if (_position.Y > _movePosition.Y)
                _position.Y -= _speed;

            if (Manager.FunctionManager.GetDistance(_position, _movePosition) < 2)
            {
                _position = _movePosition;
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    {
                        Global.MAP_POSITION.Y -= 1;

                        _movePosition = new Vector2(_position.X, _position.Y - Global.GAME_HEIGHT);
                        break;
                    }
                case Direction.Down:
                    {
                        Global.MAP_POSITION.Y += 1;

                        _movePosition = new Vector2(_position.X, _position.Y + Global.GAME_HEIGHT);
                        break;
                    }
                case Direction.Left:
                    {
                        Global.MAP_POSITION.X -= 1;

                        _movePosition = new Vector2(_position.X - Global.GAME_WIDTH, _position.Y);
                        break;
                    }
                case Direction.Right:
                    {
                        Global.MAP_POSITION.X += 1;

                        _movePosition = new Vector2(_position.X + Global.GAME_WIDTH, _position.Y);
                        break;
                    }
            }
        }

        public bool InScreenCheck(Vector2 spritePos)
        {
            Rectangle screenRectangle = new Rectangle((int)_position.X - Global.TILE_SIZE, (int)_position.Y - Global.TILE_SIZE, (int)Global.GAME_WIDTH + (2 * Global.TILE_SIZE), (int)Global.GAME_HEIGHT + (2 * Global.TILE_SIZE));
            Rectangle spriteRectangle = new Rectangle((int)spritePos.X, (int)spritePos.Y, Global.TILE_SIZE, Global.TILE_SIZE);

            return screenRectangle.Contains(spriteRectangle);
        }

        public Vector2 WorldToScreenPosition(Vector2 spritePos)
        {
            return new Vector2(spritePos.X - _position.X, spritePos.Y - _position.Y);
        }
    }
}
