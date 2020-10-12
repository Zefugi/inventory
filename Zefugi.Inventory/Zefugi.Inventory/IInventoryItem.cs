using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public interface IInventoryItem
    {
        long ID { get; set; }
        int StackSize { get; set; }
        int SlotsRequired { get; set; }
    }
}
