using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public struct Item
    {
        public long ID;

        public int Slots;
        public int StackSize;

        public string Title;
    }
}
