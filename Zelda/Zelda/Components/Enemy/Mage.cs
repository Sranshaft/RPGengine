using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zelda.Components.Items;

namespace Zelda.Components.Enemy
{
    class Mage : Component
    {
        private BaseObject _player;
        
        private Manager.MapManager _mapManager;
        
        private TimeSpan _previousRefreshTime, _refreshTime;
        
        public Mage(BaseObject player, Manager.MapManager mapManager, float tick = 2000)
        {
            _player = player;

            _mapManager = mapManager;

            _refreshTime = TimeSpan.FromMilliseconds(tick);
            _previousRefreshTime = TimeSpan.Zero;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.Enemy; }
        }

        public override void Update(GameTime gameTime)
        {
            // Check if we can shoot
            if (gameTime.TotalGameTime - _previousRefreshTime > _refreshTime)
            {
                // Reset counter
                _previousRefreshTime = gameTime.TotalGameTime;

                var equipment = GetComponent<Equipment>(ComponentType.Item);

                if (equipment != null)
                    equipment.UseItem(ItemSlot.A);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
