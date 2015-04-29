using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class PlayerInput : Component
    {
        public PlayerInput()
        {
            Manager.InputManager.EventInput += ManagerInput_EventInput;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.PlayerInput; }
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch) { }

        void ManagerInput_EventInput(object sender, EventHandlers.InputEventArgs e)
        {
            var sprite = GetComponent<Sprite>(ComponentType.Sprite);
            if (sprite == null)
                return;

            var camera = GetComponent<Camera>(ComponentType.Camera);
            if (camera == null)
                return;

            var x = 0f;
            var y = 0f;

            if (camera.InTransition())
                return;

            switch (e.Input)
            {
                case Input.Up:
                    {
                        y = -Global.PLAYER_MOVEMENT;
                        break;
                    }
                case Input.Down:
                    {
                        y = Global.PLAYER_MOVEMENT;
                        break;
                    }
                case Input.Left:
                    {
                        x = -Global.PLAYER_MOVEMENT;
                        break;
                    }
                case Input.Right:
                    {
                        x = Global.PLAYER_MOVEMENT;
                        break;
                    }
                case Input.A:
                    {
                        var equipment = GetComponent<Equipment>(ComponentType.Item);
                        equipment.UseItem(ItemSlot.A);
                        break;
                    }
                case Input.B:
                    {
                        var equipment = GetComponent<Equipment>(ComponentType.Item);
                        equipment.UseItem(ItemSlot.B);
                        break;
                    }
            }

            var collision = GetComponent<Collision>(ComponentType.Collision);

            Rectangle spriteRectangle = sprite.Bounds;
            spriteRectangle.Offset((int)x, (int)y);

            if (collision == null || !collision.CheckCollision(spriteRectangle))
                sprite.Move(x, y);

            CheckCamera(sprite);
        }

        private void CheckCamera(Sprite sprite)
        {
            var camera = GetComponent<Camera>(ComponentType.Camera);

            if (camera == null)
                return;

            Vector2 position;
            if (!camera.GetPosition(sprite.Position, out position))
            {
                var animation = GetComponent<Animation>(ComponentType.Animation);

                if (animation == null)
                    return;

                camera.Move(animation.CurrentDirection);
            }
        }
    }
}
