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

        private const float _speed = 4f;

        public Fireball()
        {
            ItemID = "projectile:Fireball";
        }

        public override void LoadContent(Equipment owner, ContentManager content, Manager.MapManager mapManager, Manager.CameraManager cameraManager)
        {
            base.LoadContent(owner, content, mapManager, cameraManager);

            Texture2D texture = content.Load<Texture2D>(string.Format("Sprites//Projectile//fireball_{0}", Global.TILE_SIZE));

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

            Move(sprite);

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

            ownerAnimation.ResetCounter(State.Special, _direction);

            var sprite = GetComponent<Sprite>(ComponentType.Sprite);

            if (sprite != null)
            {
                sprite.Teleport(ownerSprite.Position);
            }

            this.Active = true;
        }

        public void Move(Sprite sprite)
        {
            // Move the fireball in the proper direction
            switch (_direction)
            {
                case Direction.Up:
                    {
                        sprite.Move(0, -_speed);

                        break;
                    }
                case Direction.Down:
                    {
                        sprite.Move(0, _speed);

                        break;
                    }
                case Direction.Left:
                    {
                        sprite.Move(-_speed, 0);

                        break;
                    }
                case Direction.Right:
                    {
                        sprite.Move(_speed, 0);

                        break;
                    }
            }
        }
    }
}
