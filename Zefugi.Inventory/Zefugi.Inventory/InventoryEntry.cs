using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class InventoryEntry
    {
        public IItemInfo ItemInfo { get; set; }
        public int ItemCount { get; set; }

        public InventoryEntry() { }

        public InventoryEntry(IItemInfo itemInfo, int itemCount)
        {
            ItemInfo = itemInfo;
            ItemCount = itemCount;
        }
    }
}
