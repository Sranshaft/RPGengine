using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class Collision : Component
    {
        private Manager.MapManager _mapManager;

        public Collision(Manager.MapManager mapManager)
        {
            _mapManager = mapManager;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.Collision; }
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch) { }

        public bool CheckCollision(Rectangle rectangle, bool fixBox = true)
        {
            if (fixBox)
            {
                rectangle.Inflate(-(rectangle.Width / 8), -(rectangle.Height / 4));
                rectangle.Offset(0, (rectangle.Height / 2));
            }
            
            return _mapManager.CheckCollision(rectangle);
        }
    }
}
