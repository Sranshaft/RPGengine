using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda
{
    abstract class Component
    {
        private BaseObject _baseObject;

        public abstract ComponentType ComponentType { get; }

        public void Initialize(BaseObject baseObject)
        {
            _baseObject = baseObject;
        }

        public int GetObjectID()
        {
            return _baseObject.ID;
        }

        public void Remove()
        {
            _baseObject.RemoveComponent(this);
        }

        public TComponentType GetComponent<TComponentType>(ComponentType componentType) where TComponentType : Component
        {
            if (_baseObject == null)
                return null;

            try
            {
                return _baseObject.GetComponent<TComponentType>(componentType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Error returning component: {0} at {1}", ex.Message, ex.StackTrace));
                return null;
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
