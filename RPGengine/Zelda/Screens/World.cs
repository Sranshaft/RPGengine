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
using Zelda.Components.Items;

namespace Zelda.Screens
{
    class World : Screen
    {
        private Manager.EntitiesManager _entitiesManager;
        
        private Manager.MapManager _mapManager;
        private Manager.CameraManager _cameraManager;

        public World(Manager.ScreenManager screenManager)
            : base(screenManager)
        {
            // Initialize managers
            _cameraManager = new Manager.CameraManager();
            _entitiesManager = new Manager.EntitiesManager(_cameraManager);
            _mapManager = new Manager.MapManager("test", _cameraManager);
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
            // Load tile and collision maps
            _mapManager.LoadContent(content);

            // Load shadow texture to global
            Global.SHADOW_TEXTURE = content.Load<Texture2D>("Sprites//entity_shadow");

            // Load player components
            var player = new BaseObject();
            player.AddComponent(new Sprite(content.Load<Texture2D>(string.Format("Sprites//brawler_{0}", Global.TILE_SIZE)), Global.TILE_SIZE, Global.TILE_SIZE, new Vector2(64, 64), 0, true, 1));
            player.AddComponent(new PlayerInput());
            player.AddComponent(new Animation(Global.TILE_SIZE, Global.TILE_SIZE, 3));
            player.AddComponent(new Collision(_mapManager, _entitiesManager));
            player.AddComponent(new Camera(_cameraManager));
            player.AddComponent(new Equipment(content, _mapManager, _cameraManager));
            player.GetComponent<Equipment>(ComponentType.Item).AddItem(new Fireball());
            player.GetComponent<Equipment>(ComponentType.Item).EquipItemInSlot("projectile:Fireball", ItemSlot.A);
            player.GetComponent<Equipment>(ComponentType.Item).AddItem(new Boomerang());
            player.GetComponent<Equipment>(ComponentType.Item).EquipItemInSlot("projectile:Boomerang", ItemSlot.B);

            // Load test enemy components
            var enemy = new BaseObject();
            enemy.AddComponent(new Sprite(content.Load<Texture2D>(string.Format("Sprites//mage_{0}", Global.TILE_SIZE)), Global.TILE_SIZE, Global.TILE_SIZE, new Vector2(64, 128), 0, true));
            enemy.AddComponent(new AIMovementSmart(player, 1000, 2f));
            enemy.AddComponent(new Animation(Global.TILE_SIZE, Global.TILE_SIZE, 3));
            enemy.AddComponent(new Collision(_mapManager, _entitiesManager));
            enemy.AddComponent(new Mage(player, _mapManager, 500));
            enemy.AddComponent(new Camera(_cameraManager));
            enemy.AddComponent(new Equipment(content, _mapManager, _cameraManager));
            enemy.GetComponent<Equipment>(ComponentType.Item).AddItem(new Fireball());
            enemy.GetComponent<Equipment>(ComponentType.Item).EquipItemInSlot("projectile:Fireball", ItemSlot.A);

            // Load text NPC components
            BaseObject npc;

            for (int i = 0; i < 2; i++)
            {
                npc = new BaseObject();

                npc.AddComponent(new Sprite(content.Load<Texture2D>(string.Format("Sprites//knight_{0}", Global.TILE_SIZE)), Global.TILE_SIZE, Global.TILE_SIZE, new Vector2(64 * (i + 1), 128), 0, true));
                npc.AddComponent(new AIMovementRandom(2000, 1f));
                npc.AddComponent(new Animation(Global.TILE_SIZE, Global.TILE_SIZE, 3, 500));
                npc.AddComponent(new Collision(_mapManager, _entitiesManager));
                npc.AddComponent(new Camera(_cameraManager));

                _entitiesManager.AddEntity(npc);
            }

            _entitiesManager.AddEntity(enemy);
            _entitiesManager.AddEntity(player);
        }

        public override void Update(GameTime gameTime)
        {
            // Update the camera
            _cameraManager.Update(gameTime);

            // Update the map
            _mapManager.Update(gameTime);

            // Make sure camera isn't locked before updating entities
            if (!_cameraManager.Locked)
            {
                _entitiesManager.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw map tiles
            _mapManager.Draw(spriteBatch);

            // Draw entities
            _entitiesManager.Draw(spriteBatch);
        }

        void InputManager_EventInput(object sender, EventHandlers.InputEventArgs e)
        {
            if (e.Input == Input.F5)
            {
                this.ScreenManager.LoadPreviousScreen();
                Global.MAP_POSITION = Vector2.Zero;
            }
        }
    }
}
