using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    public interface IRepositorySystem
    {
        IItemInfo this[int id] { get; }
        int Count { get; }

        void Clear();
        void Add(IItemInfo item);
        void AddRange(IEnumerable<IItemInfo> list);
    }
}
