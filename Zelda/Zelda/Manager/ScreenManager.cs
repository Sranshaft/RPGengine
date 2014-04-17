using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Screens;

namespace Zelda.Manager
{
    public class ScreenManager
    {
        private Screen _currentScreen;
        private Screen _lastScreen;

        private ContentManager _content;

        public ScreenManager(ContentManager content)
        {
            _content = content;
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
        }

        public void LoadNextScreen(Screen newScreen)
        {
            // Store current screen as last screen
            _lastScreen = _currentScreen;

            // Uninitialize the previous screen
            if (_lastScreen != null)
                _lastScreen.Uninitialize();
            
            // Set current screen to new screen
            _currentScreen = newScreen;

            // Initialize current screen
            InitializeScreen(_currentScreen);   
        }

        public void LoadPreviousScreen()
        {
            if (_lastScreen == null)
                return;

            // Uninitialize the previous screen
            _currentScreen.Uninitialize();

            // Set current screen to previous screen
            _currentScreen = _lastScreen;

            InitializeScreen(_currentScreen);
        }

        public void InitializeScreen(Screen screen)
        {
            // Load current screen content
            screen.Initialize();

            // Load current screen content
            screen.LoadContent(_content);
        }
    }

}
