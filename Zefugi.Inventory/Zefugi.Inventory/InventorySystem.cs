﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class InventorySystem<TItem>
        where TItem : IItemInfo
    {
        private List<InventoryEntry> _items = new List<InventoryEntry>();

        private int _totalSlots = 1;
        private bool _autoCompress = false;

        public bool AutoCompress
        {
            get => _autoCompress;
            set
            {
                if (value && !_autoCompress)
                    Compress();

                _autoCompress = value;
            }
        }

        public int TotalSlots
        {
            get => _totalSlots;
            set
            {
                if (value < UsedSlots)
                    throw new InventoryException("Unable to shrink TotalSlots below the currently used slots.");

                _totalSlots = value;
            }
        }

        public bool Store(IItemInfo item, int amount)
        {
            if (!HasRoomFor(item, amount))
                return false;

            int requiredSlots = item.SlotsRequired * (amount / item.StackSize);
            requiredSlots += amount % item.StackSize > 0 ? 1 : 0;
            int amountRemaning = amount;
            
            foreach(var entry in _items)
            {
                if(entry.ItemInfo.ID == item.ID
                    && entry.ItemCount < entry.ItemInfo.StackSize)
                {
                    var deltaAmount = entry.ItemInfo.StackSize - entry.ItemCount;
                    deltaAmount = deltaAmount < amountRemaning ? deltaAmount : amountRemaning;

                    amountRemaning -= deltaAmount;
                    entry.ItemCount += deltaAmount;
                }
            }

            while(amountRemaning > 0)
            {
                var deltaAmount = amountRemaning <= item.StackSize ? amountRemaning : item.StackSize;

                amountRemaning -= deltaAmount;
                _items.Add(new InventoryEntry(item, deltaAmount));
            }

            if (AutoCompress) Compress();

            return true;
        }

        public bool Retrieve(IItemInfo item, int amount)
        {
            if (!IsAvailable(item, amount))
                return false;

            int amountRemaning = amount;
            
            for(int i = 0; i < _items.Count; i++)
            {
                if(_items[i].ItemInfo.ID == item.ID)
                {
                    var entry = _items[i];
                    if (amountRemaning < entry.ItemCount)
                    {
                        entry.ItemCount -= amountRemaning;
                        if (AutoCompress) Compress();
                        return true;
                    }
                    else
                    {
                        amountRemaning -= entry.ItemCount;
                        _items.RemoveAt(i--);
                        if (amountRemaning == 0)
                        {
                            if (AutoCompress) Compress();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public int UsedSlots
        {
            get
            {
                int count = 0;
                foreach (var entry in _items)
                    count += entry.ItemInfo.SlotsRequired;
                return count;
            }
        }

        public int FreeSlots => _totalSlots - UsedSlots;

        public bool IsAvailable(IItemInfo item, int amount)
        {
            int amountRemaning = amount;

            foreach(var entry in _items)
            {
                if (entry.ItemInfo.ID == item.ID)
                    amountRemaning -= entry.ItemCount;
                if (amountRemaning < 1)
                    return true;
            }

            return false;
        }

        public bool HasRoomFor(IItemInfo item, int amount)
        {
            int freeSlotsRequired = item.SlotsRequired * (amount / item.StackSize);
            int additionalStackSize = amount % item.StackSize;

            if (freeSlotsRequired > FreeSlots)
                return false;

            if (additionalStackSize == 0)
                return true;

            foreach(var entry in _items)
            {
                if (entry.ItemInfo.ID == item.ID && additionalStackSize <= item.StackSize - entry.ItemCount)
                    return true;
            }

            if (freeSlotsRequired + 1 <= FreeSlots)
                return true;

            return false;
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Compress()
        {
            List<IItemInfo> typeList = GetAllItemTypes();

            foreach(var type in typeList)
            {
                int amount = GetAmount(type.ID);
                ClearItem(type.ID);
                while(amount > 0)
                {
                    var deltaAmount = amount <= type.StackSize ? amount : type.StackSize;
                    amount -= deltaAmount;
                    _items.Add(new InventoryEntry(type, deltaAmount));
                }
            }
        }

        public int GetAmount(IItemInfo item) => GetAmount(item.ID);

        public int GetAmount(int itemID)
        {
            var amount = 0;
            foreach (var entry in _items)
                if (entry.ItemInfo.ID == itemID)
                    amount += entry.ItemCount;
            return amount;
        }

        public void ClearItem(IItemInfo item) => ClearItem(item.ID);

        public void ClearItem(int id)
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].ItemInfo.ID == id)
                    _items.RemoveAt(i--);
        }

        public List<IItemInfo> GetAllItemTypes()
        {
            List<IItemInfo> typeList = new List<IItemInfo>();
            foreach (var entry in _items)
                if (!typeList.Contains(entry.ItemInfo))
                    typeList.Add(entry.ItemInfo);
            return typeList;
        }
    }
}
