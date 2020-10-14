using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory
{
    [TestFixture]
    public class InventoryEntry_Tests
    {
        [Test]
        public void Ctor_SetsItemInfoAndItemCount()
        {
            var itemCount = 3;

            var item = Substitute.For<IItemInfo>();
            item.ID = 42;
            item.Name = "Shovel";
            item.SlotsRequired = 2;
            item.StackSize = 4;

            var entry = new InventoryEntry(item, itemCount);

            Assert.AreEqual(item, entry.ItemInfo);
            Assert.AreEqual(itemCount, entry.ItemCount);
        }
    }
}
