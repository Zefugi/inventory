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
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("SlotsTotal");

                int shrinkage = SlotsTotal - value;
                if (shrinkage > FreeSlots)
                    throw new InventoryException("Unable to shrink inventory as there are not enough free slots.");

                _numberOfSlots = value;
            }
        }

        public bool HasRoom(T item) => item.SlotsRequired <= FreeSlots;

        public bool IsAvailable(T item)
        {
            foreach (T entry in _items)
                if (item.ID == entry.ID)
                    return true;

            return false;
        }

        public void Store(T item)
        {
            if (!HasRoom(item))
                throw new InventoryException("Tried to add an item that did not fit in inventory.");
            
            _items.Add(item);
        }

        public T Drain(T item)
        {
            if (!IsAvailable(item))
                throw new InventoryException("Tried to drain an item that was not there.");

            T retrievedItem = null;
            foreach(var entry in _items)
            {
                if(entry.ID == item.ID)
                {
                    retrievedItem = entry;
                    break;
                }
            }

            _items.Remove(retrievedItem);
            return retrievedItem;
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

        public int FreeSlots => _numberOfSlots - UsedSlots;
    }
}
