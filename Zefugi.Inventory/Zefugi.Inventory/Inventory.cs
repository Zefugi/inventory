using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class Inventory
    {
        private int _slotsTotal = 1;
        private List<InventoryEntry> _items = new List<InventoryEntry>();

        public Inventory(int slotsTotal)
        {
            _slotsTotal = slotsTotal;
        }

        public int SlotsTotal
        {
            get => _slotsTotal;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("SlotsTotal");

                int shrinkage = _slotsTotal - value;
                if (shrinkage > FreeSlots)
                    throw new InventoryException("Can't shrink inventory. Not enough free slots.");

                _slotsTotal = value;
            }
        }

        public int FreeSlots => _slotsTotal - UsedSlots;

        public int UsedSlots
        {
            get
            {
                int count = 0;
                foreach (var entry in _items)
                    count += entry.Item.Slots;
                return count;
            }
        }
    }
}