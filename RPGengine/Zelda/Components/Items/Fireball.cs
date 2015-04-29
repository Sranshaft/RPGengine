using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components.Items
{
    class Fireball : Item
    {
        private Direction _direction;
        
        private TimeSpan _previousRefreshTime, _refreshTime;
        private TimeSpan _previousAnimationTime, _animationTime;

        private const float _speed = 10f;

        public Fireball()
        {
            ItemID = "projectile:Fireball";
        }

        public override void LoadContent(Equipment owner, ContentManager content, Manager.MapManager mapManager, Manager.CameraManager cameraManager)
        {
            base.LoadContent(owner, content, mapManager, cameraManager);

            Texture2D texture = content.Load<Texture2D>(string.Format("Sprites//Projectile//fireball_{0}", Global.TILE_SIZE));

            AddComponent(new Sprite(texture, texture.Width, texture.Height, Vector2.Zero));
            AddComponent(owner.GetComponent<Component>(ComponentType.Collision));
            AddComponent(new Camera(cameraManager));

            _refreshTime = TimeSpan.FromMilliseconds(750);
            _previousRefreshTime = TimeSpan.Zero;

            _animationTime = TimeSpan.FromMilliseconds(100);
            _previousAnimationTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.Active)
                return;

            base.Update(gameTime);

            _previousRefreshTime += gameTime.ElapsedGameTime;
            _previousAnimationTime += gameTime.ElapsedGameTime;

            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite == null)
                return;

            if (_previousRefreshTime > _refreshTime)
            {
                this.Active = false;
                _previousRefreshTime = TimeSpan.Zero;
                _previousAnimationTime = TimeSpan.Zero;
            }
            else
            {
                Move(sprite);
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
            if (this.Active)
                base.Draw(spriteBatch);
        }

        public override void Action()
        {     
            var ownerAnimation = this.Owner.GetComponent<Animation>(ComponentType.Animation);
            var ownerSprite = this.Owner.GetComponent<Sprite>(ComponentType.Sprite);

            if (ownerAnimation == null || ownerSprite == null)
                return;

            _direction = ownerAnimation.CurrentDirection;

            ownerAnimation.ResetCounter(State.Special, _direction);

            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite != null)
            {
                sprite.Teleport(ownerSprite.Center);
            }

            this.Active = true;
        }

        public void Move(Sprite sprite)
        {
            var x = 0f;
            var y = 0f;

            // Move the fireball in the proper direction
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
            }

            var collision = GetComponent<Collision>(ComponentType.Collision);

            Rectangle spriteRectangle = sprite.Bounds;
            spriteRectangle.Offset((int)x, (int)y);

            if (collision == null || collision.CheckCollision(spriteRectangle, false))
            {
                this.Active = false;
                _previousRefreshTime = TimeSpan.Zero;
                _previousAnimationTime = TimeSpan.Zero;
            }
            else
                sprite.Move(x, y);
        }
    }
}
