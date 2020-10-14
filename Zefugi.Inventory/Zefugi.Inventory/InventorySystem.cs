using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class InventorySystem
    {
        private List<InventoryEntry> _items = new List<InventoryEntry>();

        private int _totalSlots = 1;

        public int TotalSlots
        {
            get => _totalSlots;
            set
            {
                _totalSlots = value;
            }
        }
    }
}
