using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class Camera : Component
    {
        private Manager.CameraManager _cameraManager;

        public Camera(Manager.CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.Camera; }
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch) { }

        public void Move(Direction direction)
        {
            _cameraManager.Move(direction);
        }

        public bool InTransition()
        {
            return _cameraManager.Locked;
        }

        public bool OnScreen(Vector2 position)
        {
            return _cameraManager.InScreenCheck(position);
        }

        public bool GetPosition(Vector2 position, out Vector2 newPosition)
        {
            newPosition = _cameraManager.WorldToScreenPosition(position);
            return _cameraManager.InScreenCheck(position);
        }
    }
}
