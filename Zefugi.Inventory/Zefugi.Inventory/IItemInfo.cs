using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public interface IItemInfo
    {
        int ID { get; set; }
        string Name { get; set; }
        int SlotsRequired { get; set; }
        int StackSize { get; set; }
    }
}
