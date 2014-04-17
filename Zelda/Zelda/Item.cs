using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Components;

namespace Zelda
{
    abstract class Item : BaseObject
    {
        protected Equipment Owner;

        public string ItemID { get; set; }
        public bool Active { get; set; }

        public abstract void Action();

        public virtual void LoadContent(Equipment owner, ContentManager content, Manager.MapManager mapManager, Manager.CameraManager cameraManager)
        {
            Owner = owner;
        }
    }
}
