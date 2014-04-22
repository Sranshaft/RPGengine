using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda
{
    public abstract class Screen
    {
        protected Manager.ScreenManager ScreenManager;

        public Screen(Manager.ScreenManager screenManager)
        {
            this.ScreenManager = screenManager;
        }

        public virtual void Initialize() { }
        public virtual void Uninitialize() { }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
