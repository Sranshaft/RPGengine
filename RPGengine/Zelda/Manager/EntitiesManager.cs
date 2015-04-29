using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Components;

namespace Zelda.Manager
{
    class EntitiesManager
    {
        private List<BaseObject> _entities;
        private Manager.CameraManager _cameraManager;

        public EntitiesManager(Manager.CameraManager cameraManager)
        {
            _entities = new List<BaseObject>();
            _cameraManager = cameraManager;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in _entities)
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obj in _entities)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void AddEntity(BaseObject newObject)
        {
            _entities.Add(newObject);
        }

        public void AddEntities(List<BaseObject> newObjects)
        {
            _entities.AddRange(newObjects);
        }

        public void RemoveEntity(BaseObject obj)
        {
            _entities.Remove(obj);
        }

        public bool CheckCollision(Rectangle rectangle)
        {
            return _entities.Any(entity => entity.Intersect(rectangle, entity.GetComponent<Sprite>(ComponentType.Sprite).Position, entity.GetComponent<Camera>(ComponentType.Camera).CameraManager));
        }
    }
}
