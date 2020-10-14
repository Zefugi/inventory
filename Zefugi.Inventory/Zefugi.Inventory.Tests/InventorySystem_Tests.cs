using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory.Tests
{
    [TestFixture]
    public class InventorySystem_Tests
    {
        [Test]
        public void TotalSlots_CanGrow()
        {
            var inv = new InventorySystem();

            Assert.AreEqual(1, inv.TotalSlots);

            inv.TotalSlots = 4;

            Assert.AreEqual(4, inv.TotalSlots);
        }

        [Test]
        [TestCase(5, false)]
        [TestCase(4, false)]
        [TestCase(3, false)]
        [TestCase(2, false)]
        [TestCase(1, true)]
        public void TotalSlots_CanShrink_IfSpaceAvailable_Throws_ifSpaceUnavailable(int finalTotalSlots, bool throwException)
        {
            var initialSlots = 4;
            var inv = new InventorySystem();
            inv.TotalSlots = initialSlots;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 2;
            item.StackSize = 1;

            inv.Store(item, 1);

            if (throwException)
            {
                Assert.Throws<InventoryException>(() => { inv.TotalSlots = finalTotalSlots; });
                Assert.AreEqual(initialSlots, inv.TotalSlots);
            }
            else
            {
                inv.TotalSlots = finalTotalSlots;
                Assert.AreEqual(finalTotalSlots, inv.TotalSlots);
            }
        }

        [Test]
        public void Store_ReturnsTrueAndMakesItemAvailable_IfSpaceAvailable()
        {
            var inv = new InventorySystem();
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 1;
            itemA.StackSize = 1;

            Assert.IsTrue(inv.Store(itemA, 1));
            Assert.IsTrue(inv.Retrieve(itemA, 1));
        }

        [Test]
        public void Store_ReturnsFalseAndDoesNotStore_IfSpaceUnavailable()
        {
            var inv = new InventorySystem();
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 2;
            itemA.StackSize = 1;

            Assert.IsFalse(inv.Store(itemA, 1));
            Assert.IsFalse(inv.Retrieve(itemA, 1));

            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 1337;
            itemB.SlotsRequired = 1;
            itemB.StackSize = 5;

            Assert.IsTrue(inv.Store(itemB, 3));
            Assert.IsFalse(inv.Store(itemB, 3));
            Assert.IsFalse(inv.Retrieve(itemB, 5));
        }

        [Test] // TODO Store_Compresses_IfAutoStackIsTrue
        public void Store_Compresses_IfAutoStackIsTrue() { }

        [Test]
        public void Retrieve_ReturnsItemAndMakesItemUnavailable_IfItemIsAvailable()
        {
            var inv = new InventorySystem();
            inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 2;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            inv.Store(item, 3);

            Assert.IsTrue(inv.Retrieve(item, 2));
            Assert.IsFalse(inv.Retrieve(item, 2));

            inv.Clear();
            item.StackSize = 1;

            inv.Store(item, 2);

            Assert.IsTrue(inv.Retrieve(item, 2));
            Assert.IsFalse(inv.Retrieve(item, 1));
        }

        [Test] // TODO Retrieve_Compresses_IfAutoStackIsTrue
        public void Retrieve_Compresses_IfAutoStackIsTrue() { }

        [Test]
        public void UsedSlots_ReturnsUsedSlots()
        {
            var inv = new InventorySystem();
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            Assert.AreEqual(0, inv.UsedSlots);
            inv.Store(item, 1);
            Assert.AreEqual(1, inv.UsedSlots);
            inv.TotalSlots = 10;
            item.SlotsRequired = 4;
            Assert.AreEqual(4, inv.UsedSlots);

        }

        [Test]
        public void FreeSlots_ReturnsUnusedSlots()
        {
            var inv = new InventorySystem();
            var item = Substitute.For<IItemInfo>();
            item.SlotsRequired = 2;
            item.StackSize = 1;

            Assert.AreEqual(1, inv.FreeSlots);
            inv.TotalSlots = 5;
            Assert.AreEqual(5, inv.FreeSlots);

            inv.Store(item, 1);
            Assert.AreEqual(3, inv.FreeSlots);
        }

        [Test]
        public void Clear_MakesAllItemsUnavailable()
        {
            var inv = new InventorySystem();
            var item = Substitute.For<IItemInfo>();
            item.SlotsRequired = 1;
            item.StackSize = 1;

            inv.Store(item, 1);
            inv.Clear();

            Assert.AreEqual(1, inv.FreeSlots);
            Assert.AreEqual(0, inv.UsedSlots);
        }

        [Test]
        public void IsAvailable_ReturnsTrue_OnlyIfTheSpecifiedItemsAreAvailable()
        {
            var inv = new InventorySystem();
            var item = Substitute.For<IItemInfo>();
            item.ID = 2;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            inv.Store(item, 1);
            Assert.IsTrue(inv.IsAvailable(item, 1));
            Assert.IsFalse(inv.IsAvailable(item, 2));
            inv.Clear();

            item.StackSize = 2;
            inv.Store(item, 2);
            Assert.IsTrue(inv.IsAvailable(item, 2));
            Assert.IsFalse(inv.IsAvailable(item, 3));
        }

        [Test]
        public void HasRoomFor_ReturnsTrue_OnlyIfAllItemsSpecifiedCanBeStored()
        {
            var inv = new InventorySystem();
            var item = Substitute.For<IItemInfo>();
            item.ID = 42;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            Assert.IsTrue(inv.HasRoomFor(item, 1));
            Assert.IsFalse(inv.HasRoomFor(item, 2));
            inv.Store(item, 1);
            Assert.IsFalse(inv.HasRoomFor(item, 1));
            inv.Clear();

            item.StackSize = 2;
            Assert.IsTrue(inv.HasRoomFor(item, 2));
            Assert.IsFalse(inv.HasRoomFor(item, 3));

            item.SlotsRequired = 2;
            item.StackSize = 1;
            Assert.IsFalse(inv.HasRoomFor(item, 1));
        }

        [Test]
        public void Compress_StacksItemsToSaveSlots()
        {
            var inv = new InventorySystem();
            inv.TotalSlots = 4;
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 1;
            itemA.StackSize = 40;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 1337;
            itemB.SlotsRequired = 2;
            itemB.StackSize = 3;

            inv.Clear();
            for (int i = 0; i < 3; i++)
                inv.Store(itemA, 20);
            int preAmount = inv.GetAmount(itemA);
            inv.Compress();
            Assert.AreEqual(preAmount, inv.GetAmount(itemA));
            Assert.AreEqual(2, inv.UsedSlots);

            inv.Clear();
            for (int i = 0; i < 4; i++)
                inv.Store(itemB, 1);
            preAmount = inv.GetAmount(itemB);
            inv.Compress();
            Assert.AreEqual(preAmount, inv.GetAmount(itemB));
            Assert.AreEqual(2, inv.UsedSlots);
        }

        [Test]
        public void GetAmount_ReturnsAmountOfSpecifiedItem()
        {
            var firstAmount = 2;
            var secondAmount = 4;

            var inv = new InventorySystem();
            inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;

            item.SlotsRequired = 1;
            item.StackSize = 1;
            inv.Store(item, firstAmount);
            Assert.AreEqual(firstAmount, inv.GetAmount(item));

            item.StackSize = 5;
            inv.Store(item, secondAmount);
            Assert.AreEqual(firstAmount + secondAmount, inv.GetAmount(item));
        }

        [Test]
        public void ClearItem_MakesTheSpecifiedItemUnavailable()
        {
            var inv = new InventorySystem();
            inv.TotalSlots = 4;
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            itemA.SlotsRequired = 2;
            itemA.StackSize = 1;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;
            itemB.SlotsRequired = 1;
            itemB.StackSize = 5;

            inv.Store(itemA, 1);
            inv.Store(itemB, 10);
            inv.ClearItem(itemB);

            Assert.AreEqual(1, inv.GetAmount(itemA));
            Assert.AreEqual(0, inv.GetAmount(itemB));
        }

        [Test]
        public void AutoCompress_CompressesWhenChangedToTrue()
        {
            var inv = new InventorySystem();
            inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            inv.Store(item, 2);
            inv.Store(item, 2);

            inv.AutoCompress = true;

            Assert.AreEqual(1, inv.UsedSlots);
            Assert.AreEqual(4, inv.GetAmount(item));
        }
    }
}
