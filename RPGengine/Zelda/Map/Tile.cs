using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Map
{
    public class Tile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }

        public List<TileFrame> TileFrames { get; set; }

        public int AnimationSpeed { get; set; }

        public string TextureName { get; set; }

        private Texture2D _texture;
        private int _animationIndex;

        private TimeSpan _previousRefreshTime, _refreshTime;

        public Manager.CameraManager CameraManager { get; set; }
        public Vector2 Position { get { return new Vector2(this.PosX * Global.TILE_SIZE, this.PosY * Global.TILE_SIZE); } }

        public Tile() { }

        public Tile(int posX, int posY, int posZ, List<TileFrame> tileFrames, int animationSpeed, string textureName, Manager.CameraManager cameraManager)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.PosZ = posZ;

            this.AnimationSpeed = animationSpeed;

            this.TileFrames = tileFrames;
            this.TextureName = textureName;

            _refreshTime = TimeSpan.FromMilliseconds(animationSpeed);
            _previousRefreshTime = TimeSpan.Zero;

            _animationIndex = 0;

            CameraManager = cameraManager;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(string.Format("Tilesets//{0}_{1}", this.TextureName, Global.TILE_SIZE));
        }

        public void Update(GameTime gameTime)
        {
            if (this.TileFrames.Count <= 1)
                return;

            _previousRefreshTime += gameTime.ElapsedGameTime;

            if (_previousRefreshTime > _refreshTime)
            {
                _previousRefreshTime = TimeSpan.Zero;
                _animationIndex++;

                if (_animationIndex >= this.TileFrames.Count)
                {
                    _animationIndex = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = CameraManager.WorldToScreenPosition(this.Position);
            
            if (CameraManager.InScreenCheck(this.Position))
            {
                Rectangle sourceRect = new Rectangle(this.TileFrames[_animationIndex].PosX * Global.TILE_SIZE, this.TileFrames[_animationIndex].PosY * Global.TILE_SIZE, Global.TILE_SIZE, Global.TILE_SIZE);
                Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, Global.TILE_SIZE, Global.TILE_SIZE);

                spriteBatch.Draw(_texture, destinationRect, sourceRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, this.PosZ);
            }
        }
    }
}
