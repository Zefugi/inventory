using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class InventoryItemBase : IInventoryItem
    {
        public long ID { get; set; }
        public int StackSize { get; set; }
        public int SlotsRequired { get; set; }
    }
}
