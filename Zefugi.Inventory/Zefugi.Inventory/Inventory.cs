using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class Inventory<T>
        where T : class, IInventoryItem, new()
    {
        private int _numberOfSlots;
        private List<T> _items = new List<T>();

        public Inventory(int numberOfSlots)
        {
            _numberOfSlots = numberOfSlots;
        }

        public int SlotsTotal
        {
            get => _numberOfSlots;
        }

        public bool HasRoom(T item)
        {
            if (_items.Count + item.SlotsRequired > _numberOfSlots)
                return false;

            return true;
        }

        public bool IsAvailable(T item)
        {
            foreach (T entry in _items)
                if (item.ID == entry.ID)
                    return true;

            return false;
        }

        public void Store(T Item)
        {
            if (!HasRoom(Item))
                throw new InventoryException("Tried to add an item that did not fit in inventory.");
            
            _items.Add(Item);
        }

        public int UsedSlots
        {
            get
            {
                int count = 0;
                foreach (var entry in _items)
                    count += entry.SlotsRequired;
                return count;
            }
        }
    }
}
