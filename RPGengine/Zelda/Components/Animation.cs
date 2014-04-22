using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class Animation : Component
    {
        public Rectangle TextureRectangle { get; private set; }

        private Direction _lastDirection;
        
        private Direction _currentDirection;
        public Direction CurrentDirection
        {
            get
            {
                if (_currentDirection != Direction.None)
                    return _currentDirection;
                else
                    return _lastDirection;
            }
        }

        private State _currentState;

        private TimeSpan _previousRefreshTime, _refreshTime;
        
        private int _frameIndex, _frameWidth, _frameHeight, _frameCount;
        
        public override ComponentType ComponentType
        {
            get { return ComponentType.Animation; }
        }

        public Animation(int frameWidth, int frameHeight, int frameCount, int tick = 100)
        {
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameCount = frameCount;
            _frameIndex = 0;
            
            _refreshTime = TimeSpan.FromMilliseconds(tick);
            _previousRefreshTime = TimeSpan.Zero;
            
            _currentState = State.Standing;
            _currentDirection = Direction.None;

            this.TextureRectangle = new Rectangle(0, 0, _frameWidth, _frameHeight);
        }

        public override void Update(GameTime gameTime) 
        {
            switch (_currentState)
            {
                case State.Walking:
                    {
                        _previousRefreshTime += gameTime.ElapsedGameTime;

                        ChangeState();

                        if (_previousRefreshTime > _refreshTime)
                        {
                            if (_frameIndex < _frameCount - 1)
                                _frameIndex++;
                            else
                                _frameIndex = 0;
                            
                            _previousRefreshTime = TimeSpan.Zero;
                        }
                        break;
                    }
            }
        }

        public void ResetCounter(State state, Direction direction)
        {
            if (direction != _currentDirection)
            {
                _previousRefreshTime = TimeSpan.Zero;
                _frameIndex = 0;
            }

            _lastDirection = _currentDirection;

            _currentState = state;
            _currentDirection = direction;
        }

        private void ChangeState()
        {
            switch (_currentDirection)
            {
                case Direction.Up:
                    {
                        this.TextureRectangle = new Rectangle(_frameWidth * _frameIndex, _frameHeight * 3, _frameWidth, _frameHeight);
                        break;
                    }
                case Direction.Down:
                    {
                        this.TextureRectangle = new Rectangle(_frameWidth * _frameIndex, 0, _frameWidth, _frameHeight);
                        break;
                    }
                case Direction.Left:
                    {
                        this.TextureRectangle = new Rectangle(_frameWidth * _frameIndex, _frameHeight, _frameWidth, _frameHeight);
                        break;
                    }
                case Direction.Right:
                    {
                        this.TextureRectangle = new Rectangle(_frameWidth * _frameIndex, _frameHeight * 2, _frameWidth, _frameHeight);
                        break;
                    }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
