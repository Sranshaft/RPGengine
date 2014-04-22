using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components.Movement
{
    class AIMovementSmart : Component
    {
        private BaseObject _player;

        private Direction _currentDirection;
        private float _speed;

        private TimeSpan _previousRefreshTime, _refreshTime;

        public AIMovementSmart(BaseObject player, int tick, float speed = Global.NPC_MOVEMENT)
        {
            _player = player;

            _currentDirection = Direction.None;
            _speed = speed;

            _refreshTime = TimeSpan.FromMilliseconds(tick);
            _previousRefreshTime = TimeSpan.Zero;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.AIMovementSmart; }
        }

        public override void Update(GameTime gameTime)
        {
            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite == null)
                return;

            var camera = GetComponent<Camera>(ComponentType.Camera);
            Vector2 position;

            if (!(camera != null && camera.GetPosition(sprite.Position, out position)))
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
