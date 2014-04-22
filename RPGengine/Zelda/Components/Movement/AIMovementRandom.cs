using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components.Movement
{
    class AIMovementRandom : Component
    {
        private Direction _currentDirection;
        private float _speed;

        private TimeSpan _previousRefreshTime, _refreshTime;

        public AIMovementRandom(int tick, float speed = Global.NPC_MOVEMENT)
        {
            _currentDirection = Direction.None;
            _speed = speed;

            _refreshTime = TimeSpan.FromMilliseconds(tick);
            _previousRefreshTime = TimeSpan.Zero;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.AIMovementRandom; }
        }

        public override void Update(GameTime gameTime)
        {
            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite == null)
                return;

            var camera = GetComponent<Camera>(ComponentType.Camera);
            
            if (camera == null)
                return;

            if (!camera.OnScreen(sprite.Position) || camera.InTransition())
                return;

            _previousRefreshTime += gameTime.ElapsedGameTime;

            if (_previousRefreshTime > _refreshTime)
                ChangeDirection();

            var collision = GetComponent<Collision>(ComponentType.Collision);

            var x = 0f;
            var y = 0f;

            switch (_currentDirection)
            {
                case Direction.Up:
                    {
                        y = -_speed;
                        break;
                    }
                case Direction.Down:
                    {
                        y = _speed;
                        break;
                    }
                case Direction.Left:
                    {
                        x = -_speed;
                        break;
                    }
                case Direction.Right:
                    {
                        x = _speed;
                        break;
                    }
                case Direction.None:
                    {
                        x = 0;
                        y = 0;
                        break;
                    }
            }

            Rectangle spriteRectangle = sprite.Bounds;
            spriteRectangle.Offset((int)x, (int)y);

            if (collision.CheckCollision(spriteRectangle))
            {
                ChangeDirection();
                return;
            }

            sprite.Move(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch) { }

        private void ChangeDirection()
        {
            _previousRefreshTime = TimeSpan.Zero;
            _currentDirection = (Direction)Manager.FunctionManager.Random(0, 5);
        }
    }
}
