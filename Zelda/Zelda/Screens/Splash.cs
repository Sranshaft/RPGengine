using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Components;
using Zelda.Components.Enemy;
using Zelda.Components.Movement;

namespace Zelda.Screens
{
    class Splash : Screen
    {
        private SpriteFont _fontLarge;
        private SpriteFont _fontSmall;

        public Splash(Manager.ScreenManager screenManager) : base(screenManager)
        {

        }

        public override void Initialize()
        {
            Manager.InputManager.EventInput += InputManager_EventInput;

            base.Initialize();
        }

        public override void Uninitialize()
        {
            Manager.InputManager.EventInput -= InputManager_EventInput;

            base.Uninitialize();
        }

        public override void LoadContent(ContentManager content)
        {
            _fontLarge = content.Load<SpriteFont>("Fonts//splashFont_Large");
            _fontSmall = content.Load<SpriteFont>("Fonts//splashFont_Small");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 fontLargePos = _fontLarge.MeasureString("RPG Engine") / 2;
            Vector2 fontSmallPos = _fontSmall.MeasureString("- Press Enter -") / 2;

            spriteBatch.DrawString(_fontLarge, "RPG Engine", new Vector2(Global.GAME_WIDTH / 2 - fontLargePos.X, Global.GAME_HEIGHT / 2 - fontLargePos.Y / 2 - 40), Color.DodgerBlue);
            spriteBatch.DrawString(_fontSmall, "- Press Enter -", new Vector2(Global.GAME_WIDTH / 2 - fontSmallPos.X, Global.GAME_HEIGHT / 2 - fontSmallPos.Y / 2 + 40), Color.White);
        }

        void InputManager_EventInput(object sender, EventHandlers.InputEventArgs e)
        {
            if (e.Input == Input.Enter)
            {
                this.ScreenManager.LoadNextScreen(new Screens.World(this.ScreenManager));
            }
        }
    }
}
