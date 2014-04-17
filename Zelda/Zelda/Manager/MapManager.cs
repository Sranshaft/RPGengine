using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Map;

namespace Zelda.Manager
{
    class MapManager
    {
        private List<Tile> _tiles;
        private List<TileCollision> _tileCollisions;

        private Manager.CameraManager _cameraManager;

        private string _mapName;

        public MapManager(string mapName, Manager.CameraManager cameraManager)
        {
            _tiles = new List<Tile>();
            _tileCollisions = new List<TileCollision>();

            _cameraManager = cameraManager;
            _mapName = mapName;
        }

        public void LoadContent(ContentManager content)
        {
            // Load tiles from xml
            var tiles = new List<Tile>();
            XMLSerialization.LoadXML(out tiles, string.Format("Content\\{0}_map.xml", _mapName));

            if (tiles != null)
            {
                _tiles = tiles;
                //_tiles.Sort((n, i) => n.PosZ > i.PosZ ? 1 : 0);

                foreach (var tile in _tiles)
                {
                    tile.LoadContent(content);
                    tile.CameraManager = _cameraManager;
                }
            }

            // Load collisions from xml
            var tileCollision = new List<TileCollision>();
            XMLSerialization.LoadXML(out tileCollision, string.Format("Content\\{0}_map_collision.xml", _mapName));

            if (tileCollision != null)
            {
                _tileCollisions = tileCollision;
                _tileCollisions.ForEach(t => t.CameraManager = _cameraManager);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in _tiles)
            {
                tile.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public bool CheckCollision(Rectangle rectangle)
        {
            return _tileCollisions.Any(tile => tile.Intersect(rectangle));
        }
    }
}
