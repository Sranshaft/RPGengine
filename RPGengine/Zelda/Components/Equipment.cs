using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Components
{
    class Equipment : Component
    {
        private List<Item> _items;
        private Dictionary<ItemSlot, Item> _equipedItems;

        private ContentManager _content;

        private Manager.MapManager _mapManager;
        private Manager.CameraManager _cameraManager;

        public Equipment(ContentManager content, Manager.MapManager mapManager, Manager.CameraManager cameraManager)
        {
            _content = content;
            _mapManager = mapManager;
            _cameraManager = cameraManager;

            _items = new List<Item>();
            _equipedItems = new Dictionary<ItemSlot, Item>();
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.Item; }
        }

        public void AddItem(Item item)
        {
            _items.Add(item);

            item.LoadContent(this, _content, _mapManager, _cameraManager);
        }

        public void AddItems(List<Item> items)
        {
            _items.AddRange(items);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public void EquipItemInSlot(string id, ItemSlot itemSlot)
        {
            var item = _items.FirstOrDefault(i => i.ItemID == id);

            if (item != null)
            {
                if (_equipedItems.ContainsKey(itemSlot))
                    _equipedItems[itemSlot] = item;
                else
                    _equipedItems.Add(itemSlot, item);
            }
        }

        public void UseItem(ItemSlot itemSlot)
        {
            if (_equipedItems.ContainsKey(itemSlot))
            {
                if (!_equipedItems[itemSlot].Active)
                    _equipedItems[itemSlot].Action();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var item in _equipedItems)
            {
                if (item.Value.Active)
                    item.Value.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) 
        {
            foreach (var item in _equipedItems)
            {
                if (item.Value.Active)
                    item.Value.Draw(spriteBatch);
            }
        }
    }
}
