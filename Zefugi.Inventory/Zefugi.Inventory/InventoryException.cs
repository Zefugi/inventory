using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class InventoryException : Exception
    {
        public InventoryException(string message) : base(message) { }
    }
}
