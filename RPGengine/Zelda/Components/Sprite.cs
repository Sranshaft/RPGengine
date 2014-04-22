using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class Sprite : Component
    {
        private Texture2D _texture;
        private int _width, _height;
        private bool _hasShadow;

        private float _rotation;
        public float Rotation { get { return _rotation; } set { _rotation = value; } }

        private Vector2 _position;
        public Vector2 Position { get { return _position; } set { _position = value; } }

        private Vector2 _origin;
        public Vector2 Origin { get { return _origin; } set { if (value != null) _origin = value; } }

        public Rectangle Bounds { get { return new Rectangle((int)_position.X, (int)_position.Y, _width, _height); } }

        public Sprite(Texture2D texture, int width, int height, Vector2 position, float rotation = 0, bool hasShadow = false)
        {
            _texture = texture;
            _width = width;
            _height = height;
            _position = position;
            _rotation = rotation;
            _origin = Vector2.Zero;
            _hasShadow = hasShadow;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.Sprite; }
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // CHeck camera position and adjust the sprite
            var camera = GetComponent<Camera>(ComponentType.Camera);
            Vector2 position = Vector2.Zero;
            
            if (camera == null)
                position = _position;

            if (camera != null && !camera.GetPosition(_position, out position))
                return;

            // Draw shadow
            if (_hasShadow)
                DrawShadow(position, spriteBatch);

            // Check the animation and adjust the sprite
            var animation = GetComponent<Animation>(ComponentType.Animation);

            Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, _width, _height);
            Rectangle sourceRect = new Rectangle(0, 0, _width, _height);

            if (animation != null)
                sourceRect = animation.TextureRectangle;

            spriteBatch.Draw(_texture, destinationRect, sourceRect, Color.White, _rotation, _origin, SpriteEffects.None, 0);
        }

        public void DrawShadow(Vector2 position, SpriteBatch spriteBatch)
        {
            Rectangle shadowRectangle = new Rectangle((int)(position.X + ((_width / 2) - (Global.SHADOW_TEXTURE.Width / 2))), (int)(position.Y + _height - Global.SHADOW_TEXTURE.Height + 5), Global.SHADOW_TEXTURE.Width, Global.SHADOW_TEXTURE.Height);
            spriteBatch.Draw(Global.SHADOW_TEXTURE, shadowRectangle, Color.White);
        }

        public void Move(float x, float y)
        {
            _position = new Vector2(_position.X + x, _position.Y + y);

            var animation = GetComponent<Animation>(ComponentType.Animation);

            if (animation == null)
                return;

            if (x > 0)
            {
                animation.ResetCounter(State.Walking, Direction.Right);
                return;
            }
            else if (x < 0)
            {
                animation.ResetCounter(State.Walking, Direction.Left);
                return;
            }
            else if (y > 0)
            {
                animation.ResetCounter(State.Walking, Direction.Down);
                return;
            }
            else if (y < 0)
            {
                animation.ResetCounter(State.Walking, Direction.Up);
                return;
            }
            else
            {
                animation.ResetCounter(State.Standing, Direction.None);
                return;
            }
        }

        public void Teleport(Vector2 position)
        {
            _position = position;
        }
    }
}
