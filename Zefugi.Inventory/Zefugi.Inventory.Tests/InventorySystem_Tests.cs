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
        private InventorySystem<IItemInfo> _inv;
        
        [SetUp]
        public void TestSetup()
        {
            _inv = new InventorySystem<IItemInfo>();
        }

        [TearDown]
        public void TestTearDown()
        {
            _inv.Clear();
        }

        [Test]
        public void TotalSlots_CanGrow()
        {
            Assert.AreEqual(1, _inv.TotalSlots);

            _inv.TotalSlots = 4;

            Assert.AreEqual(4, _inv.TotalSlots);
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
            _inv.TotalSlots = initialSlots;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 2;
            item.StackSize = 1;

            _inv.Store(item, 1);

            if (throwException)
            {
                Assert.Throws<InventoryException>(() => { _inv.TotalSlots = finalTotalSlots; });
                Assert.AreEqual(initialSlots, _inv.TotalSlots);
            }
            else
            {
                _inv.TotalSlots = finalTotalSlots;
                Assert.AreEqual(finalTotalSlots, _inv.TotalSlots);
            }
        }

        [Test]
        public void Store_ReturnsTrueAndMakesItemAvailable_IfSpaceAvailable()
        {
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 1;
            itemA.StackSize = 1;

            Assert.IsTrue(_inv.Store(itemA, 1));
            Assert.IsTrue(_inv.Retrieve(itemA, 1));
        }

        [Test]
        public void Store_ReturnsFalseAndDoesNotStore_IfSpaceUnavailable()
        {
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 2;
            itemA.StackSize = 1;

            Assert.IsFalse(_inv.Store(itemA, 1));
            Assert.IsFalse(_inv.Retrieve(itemA, 1));

            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 1337;
            itemB.SlotsRequired = 1;
            itemB.StackSize = 5;

            Assert.IsTrue(_inv.Store(itemB, 3));
            Assert.IsFalse(_inv.Store(itemB, 3));
            Assert.IsFalse(_inv.Retrieve(itemB, 5));
        }

        [Test]
        public void Store_Compresses_IfAutoStackIsTrue()
        {
            _inv.TotalSlots = 4;
            _inv.AutoCompress = true;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            _inv.Store(item, 2);
            _inv.Store(item, 2);
            
            Assert.AreEqual(1, _inv.UsedSlots);
            Assert.AreEqual(4, _inv.GetAmount(item));
        }

        [Test]
        public void Retrieve_ReturnsItemAndMakesItemUnavailable_IfItemIsAvailable()
        {
            _inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 2;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            _inv.Store(item, 3);

            Assert.IsTrue(_inv.Retrieve(item, 2));
            Assert.IsFalse(_inv.Retrieve(item, 2));

            _inv.Clear();
            item.StackSize = 1;

            _inv.Store(item, 2);

            Assert.IsTrue(_inv.Retrieve(item, 2));
            Assert.IsFalse(_inv.Retrieve(item, 1));
        }

        [Test]
        public void Retrieve_Compresses_IfAutoStackIsTrue()
        {
            _inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            _inv.Store(item, 2);
            _inv.Store(item, 2);
            _inv.AutoCompress = true;
            _inv.Retrieve(item, 1);

            Assert.AreEqual(1, _inv.UsedSlots);
            Assert.AreEqual(3, _inv.GetAmount(item));
        }

        [Test]
        public void UsedSlots_ReturnsUsedSlots()
        {
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            Assert.AreEqual(0, _inv.UsedSlots);
            _inv.Store(item, 1);
            Assert.AreEqual(1, _inv.UsedSlots);
            _inv.TotalSlots = 10;
            item.SlotsRequired = 4;
            Assert.AreEqual(4, _inv.UsedSlots);

        }

        [Test]
        public void FreeSlots_ReturnsUnusedSlots()
        {
            var item = Substitute.For<IItemInfo>();
            item.SlotsRequired = 2;
            item.StackSize = 1;

            Assert.AreEqual(1, _inv.FreeSlots);
            _inv.TotalSlots = 5;
            Assert.AreEqual(5, _inv.FreeSlots);

            _inv.Store(item, 1);
            Assert.AreEqual(3, _inv.FreeSlots);
        }

        [Test]
        public void Clear_MakesAllItemsUnavailable()
        {
            var item = Substitute.For<IItemInfo>();
            item.SlotsRequired = 1;
            item.StackSize = 1;

            _inv.Store(item, 1);
            _inv.Clear();

            Assert.AreEqual(1, _inv.FreeSlots);
            Assert.AreEqual(0, _inv.UsedSlots);
        }

        [Test]
        public void IsAvailable_ReturnsTrue_OnlyIfTheSpecifiedItemsAreAvailable()
        {
            var item = Substitute.For<IItemInfo>();
            item.ID = 2;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            _inv.Store(item, 1);
            Assert.IsTrue(_inv.IsAvailable(item, 1));
            Assert.IsFalse(_inv.IsAvailable(item, 2));
            _inv.Clear();

            item.StackSize = 2;
            _inv.Store(item, 2);
            Assert.IsTrue(_inv.IsAvailable(item, 2));
            Assert.IsFalse(_inv.IsAvailable(item, 3));
        }

        [Test]
        public void HasRoomFor_ReturnsTrue_OnlyIfAllItemsSpecifiedCanBeStored()
        {
            var item = Substitute.For<IItemInfo>();
            item.ID = 42;
            item.SlotsRequired = 1;
            item.StackSize = 1;

            Assert.IsTrue(_inv.HasRoomFor(item, 1));
            Assert.IsFalse(_inv.HasRoomFor(item, 2));
            _inv.Store(item, 1);
            Assert.IsFalse(_inv.HasRoomFor(item, 1));
            _inv.Clear();

            item.StackSize = 2;
            Assert.IsTrue(_inv.HasRoomFor(item, 2));
            Assert.IsFalse(_inv.HasRoomFor(item, 3));

            item.SlotsRequired = 2;
            item.StackSize = 1;
            Assert.IsFalse(_inv.HasRoomFor(item, 1));
        }

        [Test]
        public void HasRoomFor_ReturnsTrue_IfAdditionalStackSizeFitsInEmptySlot()
        {
            _inv.TotalSlots = 2;
            var item = Substitute.For<IItemInfo>();
            item.ID = 42;
            item.SlotsRequired = 1;
            item.StackSize = 4;

            _inv.Store(item, 4);
            Assert.IsTrue(_inv.HasRoomFor(item, 3));
        }

        [Test]
        public void Compress_StacksItemsToSaveSlots()
        {
            _inv.TotalSlots = 4;
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 42;
            itemA.SlotsRequired = 1;
            itemA.StackSize = 40;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 1337;
            itemB.SlotsRequired = 2;
            itemB.StackSize = 3;

            _inv.Clear();
            for (int i = 0; i < 3; i++)
                _inv.Store(itemA, 20);
            _inv.Retrieve(itemA, 20);
            int preAmount = _inv.GetAmount(itemA);
            _inv.Compress();
            Assert.AreEqual(preAmount, _inv.GetAmount(itemA));
            Assert.AreEqual(1, _inv.UsedSlots);

            _inv.Clear();
            for (int i = 0; i < 5; i++)
                _inv.Store(itemB, 1);
            _inv.Retrieve(itemB, 2);
            preAmount = _inv.GetAmount(itemB);
            _inv.Compress();
            Assert.AreEqual(preAmount, _inv.GetAmount(itemB));
            Assert.AreEqual(2, _inv.UsedSlots);
        }

        [Test]
        public void GetAmount_ReturnsAmountOfSpecifiedItem()
        {
            var firstAmount = 2;
            var secondAmount = 4;

            _inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;

            item.SlotsRequired = 1;
            item.StackSize = 1;
            _inv.Store(item, firstAmount);
            Assert.AreEqual(firstAmount, _inv.GetAmount(item));

            item.StackSize = 5;
            _inv.Store(item, secondAmount);
            Assert.AreEqual(firstAmount + secondAmount, _inv.GetAmount(item));
        }

        [Test]
        public void ClearItem_MakesTheSpecifiedItemUnavailable()
        {
            _inv.TotalSlots = 4;
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            itemA.SlotsRequired = 2;
            itemA.StackSize = 1;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;
            itemB.SlotsRequired = 1;
            itemB.StackSize = 5;

            _inv.Store(itemA, 1);
            _inv.Store(itemB, 10);
            _inv.ClearItem(itemB);

            Assert.AreEqual(1, _inv.GetAmount(itemA));
            Assert.AreEqual(0, _inv.GetAmount(itemB));
        }

        [Test]
        public void AutoCompress_CompressesWhenChangedToTrue()
        {
            _inv.TotalSlots = 4;
            var item = Substitute.For<IItemInfo>();
            item.ID = 1;
            item.SlotsRequired = 1;
            item.StackSize = 5;

            _inv.Store(item, 2);
            _inv.Store(item, 2);

            _inv.AutoCompress = true;

            Assert.AreEqual(1, _inv.UsedSlots);
            Assert.AreEqual(4, _inv.GetAmount(item));
        }

        [Test]
        public void GetAllItemTypes_ReturnsAllTypesOfItem()
        {
            _inv.TotalSlots = 4;
            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            itemA.SlotsRequired = 1;
            itemA.StackSize = 5;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;
            itemB.SlotsRequired = 2;
            itemB.StackSize = 1;

            _inv.Store(itemA, 2);
            _inv.Store(itemA, 3);
            _inv.Store(itemB, 1);
            _inv.Store(itemB, 1);

            var types = _inv.GetAllItemTypes();
            Assert.AreEqual(2, types.Count);
            Assert.IsTrue(types.Contains(itemA));
            Assert.IsTrue(types.Contains(itemB));
        }
    }
}
