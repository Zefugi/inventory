using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public class RepositorySystem : IRepositorySystem
    {
        private Dictionary<int, IItemInfo> _items = new Dictionary<int, IItemInfo>();

        public IItemInfo this[int id] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Add(IItemInfo item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<IItemInfo> list)
        {
            throw new NotImplementedException();
        }
    }
}
