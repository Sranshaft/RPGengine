using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Zelda.Components;
using Zelda.Components.Enemy;
using Zelda.Components.Movement;

namespace Zelda
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Manager.ScreenManager _managerScreen;
        private Manager.InputManager _managerInput;

        private Helpers.FPS _fps;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Global.GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = Global.GAME_HEIGHT;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.PreferMultiSampling = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize managers
           _managerInput = new Manager.InputManager();
            
            // Initialize FPS monitor
            _fps = new Helpers.FPS();

            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Global.WINDOW_WIDTH = this.Window.ClientBounds.Width;
            Global.WINDOW_HEIGHT = this.Window.ClientBounds.Height;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _managerScreen = new Manager.ScreenManager(Content);
            _managerScreen.LoadNextScreen(new Screens.Splash(_managerScreen));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);

            // Update input
            _managerInput.Update(gameTime);
            _managerScreen.Update(gameTime);

            // Update the FPS monitor
            _fps.Update();

            this.Window.Title = string.Format("{0} | {1} | {2} fps | {3}", Global.WINDOW_TITLE, Global.MAP_POSITION, Global.DEBUG_FPS.ToString("00"), gameTime.IsRunningSlowly);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _managerScreen.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
