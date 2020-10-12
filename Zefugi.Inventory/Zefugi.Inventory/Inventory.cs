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
        //private List<T> _items = new List<T>();

        public Inventory(int numberOfSlots)
        {
            _numberOfSlots = numberOfSlots;
        }

        public int SlotsTotal
        {
            get => _numberOfSlots;
        }
    }
}
