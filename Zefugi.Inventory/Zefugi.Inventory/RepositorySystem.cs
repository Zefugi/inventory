using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class RepositorySystem
    {
        private Dictionary<int, IItemInfo> _items = new Dictionary<int, IItemInfo>();

        public IItemInfo this[int id]
        {
            get => _items[id];
        }

        public int Count => throw new NotImplementedException();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Add(IItemInfo item)
        {
            _items[item.ID] = item;
        }

        public void AddRange(IEnumerable<IItemInfo> list)
        {
            foreach (var item in list)
                Add(item);
        }
    }
}
