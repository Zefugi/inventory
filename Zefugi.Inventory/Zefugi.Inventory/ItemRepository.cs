using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class ItemRepository
    {
        static ItemRepository()
        {
            _items.Add(42, new Item
            {
                ID = 42,
                Title = "Shovel",
                Slots = 3,
                StackSize = 1,
            });

            _items.Add(1337, new Item
            {
                ID = 1337,
                Title = "Rock",
                Slots = 1,
                StackSize = 5,
            });

            _items.Add(2222, new Item
            {
                ID = 2222,
                Title = "Revolver",
                Slots = 1,
                StackSize = 1,
            });
        }

        private static Dictionary<long, Item> _items = new Dictionary<long, Item>();

        public static Item GetItem(long id)
        {
            return _items[id];
        }
    }
}
