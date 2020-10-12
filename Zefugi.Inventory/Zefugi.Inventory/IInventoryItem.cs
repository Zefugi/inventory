using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public interface IInventoryItem
    {
        int StackSize { get; set; }
        int SlotsRequired { get; set; }
    }
}
