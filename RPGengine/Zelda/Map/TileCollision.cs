using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Map
{
    public class TileCollision
    {
        private Manager.CameraManager _cameraManager;
        public Manager.CameraManager CameraManager { get { return _cameraManager; } set { _cameraManager = value; } }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Vector2 Position { get { return new Vector2(this.PosX * Global.TILE_SIZE, this.PosY * Global.TILE_SIZE); } }

        public Rectangle Bounds { get { return new Rectangle(this.PosX * Global.TILE_SIZE, this.PosY * Global.TILE_SIZE, Global.TILE_SIZE, Global.TILE_SIZE); } }

        public TileCollision() { }

        public TileCollision(Manager.CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public bool Intersect(Rectangle rect)
        {
            var position = _cameraManager.WorldToScreenPosition(this.Position);
            return _cameraManager.InScreenCheck(position) && rect.Intersects(new Rectangle((int)position.X, (int)position.Y, Global.TILE_SIZE, Global.TILE_SIZE));
        }
    }
}
