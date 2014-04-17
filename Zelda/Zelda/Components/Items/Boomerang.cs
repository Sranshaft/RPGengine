using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components.Items
{
    class Boomerang : Item
    {
        private Direction _direction;
        private BoomerangState _state;
        
        private TimeSpan _previousRefreshTime, _refreshTime;
        private TimeSpan _previousAnimationTime, _animationTime;

        private const float _speed = 10f;

        private enum BoomerangState
        {
            Forward,
            Back,
            Stop
        };

        public Boomerang()
        {
            ItemID = "projectile:Boomerang";
        }

        public override void LoadContent(Equipment owner, ContentManager content, Manager.MapManager mapManager, Manager.CameraManager cameraManager)
        {
            base.LoadContent(owner, content, mapManager, cameraManager);

            Texture2D texture = content.Load<Texture2D>(string.Format("Sprites//Projectile//boomerang_{0}", Global.TILE_SIZE));

            AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero));
            AddComponent(new Collision(mapManager));
            AddComponent(new Camera(cameraManager));

            _refreshTime = TimeSpan.FromMilliseconds(1000);
            _previousRefreshTime = TimeSpan.Zero;

            _animationTime = TimeSpan.FromMilliseconds(100);
            _previousAnimationTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            _previousRefreshTime += gameTime.ElapsedGameTime;
            _previousAnimationTime += gameTime.ElapsedGameTime;
            
            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite == null)
                return;

            switch (_state)
            {
                case BoomerangState.Back:
                    {
                        MoveBack(sprite);
                        break;
                    }
                case BoomerangState.Forward:
                    {
                        MoveForward(sprite);

                        if (_previousRefreshTime > _refreshTime)
                        {
                            _state = BoomerangState.Back;
                            _previousRefreshTime = TimeSpan.Zero;
                        }

                        break;
                    }
            }

            if (_previousAnimationTime > _animationTime)
            {
                sprite.Origin = new Vector2(sprite.Bounds.Width / 2, sprite.Bounds.Height / 2);
                sprite.Rotation = sprite.Rotation == 360 ? 0 : sprite.Rotation + 1;
                _previousAnimationTime = TimeSpan.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Action()
        {
            var ownerAnimation = this.Owner.GetComponent<Animation>(ComponentType.Animation);
            var ownerSprite = this.Owner.GetComponent<Sprite>(ComponentType.Sprite);

            if (ownerAnimation == null || ownerSprite == null)
                return;

            _direction = ownerAnimation.CurrentDirection;
            _state = BoomerangState.Forward;

            ownerAnimation.ResetCounter(State.Special, _direction);

            var sprite = GetComponent<Sprite>(ComponentType.Sprite);
            
            if (sprite != null)
            {
                sprite.Teleport(ownerSprite.Position);
            }

            this.Active = true;
        }

        private void MoveForward(Sprite sprite)
        {
            var x = 0f;
            var y = 0f;

            switch (_direction)
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
                        return;
                    }
            }

            var collision = GetComponent<Collision>(ComponentType.Collision);

            Rectangle spriteRectangle = sprite.Bounds;
            spriteRectangle.Offset((int)x, (int)y);

            if (collision == null || collision.CheckCollision(spriteRectangle, false))
                _state = BoomerangState.Back;
            else
                sprite.Move(x, y);
        }

        private void MoveBack(Sprite sprite)
        {
            var ownerSprite = this.Owner.GetComponent<Sprite>(ComponentType.Sprite);

            if (ownerSprite == null)
            {
                this.Active = false;
                _state = BoomerangState.Stop;
            }

            if (Manager.FunctionManager.Distance(sprite.Position, ownerSprite.Position) < .5f)
            {
                this.Active = false;
                _state = BoomerangState.Stop;

                return;
            }

            if (ownerSprite.Position.X < sprite.Position.X)
                sprite.Move(-_speed, 0);
            if (ownerSprite.Position.X > sprite.Position.X)
                sprite.Move(_speed, 0);
            if (ownerSprite.Position.Y < sprite.Position.Y)
                sprite.Move(0, -_speed);
            if (ownerSprite.Position.Y > sprite.Position.Y)
                sprite.Move(0, _speed);
        }
    }
}
