using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zefugi.Inventory.Tests
{
    [TestFixture]
    public class Inventory_Tests
    {
        [Test]
        public void Ctor_SetsNumberOfSlots()
        {
            int numberOfSlots = 4;
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);

            Assert.AreEqual(numberOfSlots, inv.SlotsTotal);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void SlotsTotal_GetsNumberOfSlotsInTotal(int numberOfSlots)
        {
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);

            Assert.AreEqual(numberOfSlots, inv.SlotsTotal);
        }

        [Test]
        public void SlotsTotal_SetThrowsException_IfSlotsInUse()
        {
            int numberOfSlots = 4;
            int slotsPerItem = 2;
            int newNumberOfSlots = 1;
            
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            inv.Store(new InventoryItemBase() { SlotsRequired = slotsPerItem });

            Assert.Throws<InventoryException>(() => { inv.SlotsTotal = newNumberOfSlots; });
        }

        [Test]
        [TestCase(1, 1, true)]
        [TestCase(3, 2, false)]
        public void HasRoom_ReturnsSlotAvailabilityForSpecifiedItem(int firstItemSlotsRequired, int secondItemSlotsRequired, bool expectedHasRoom)
        {
            int numberOfSlots = 4;
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            inv.Store(new InventoryItemBase() { SlotsRequired = firstItemSlotsRequired });

            bool hasRoom = inv.HasRoom(new InventoryItemBase()
            {
                SlotsRequired = secondItemSlotsRequired,
            });

            Assert.AreEqual(expectedHasRoom, hasRoom);
        }

        [Test] // TODO HasRoom_ReturnsStackAvailabilityForSpecifiedItem
        public void HasRoom_ReturnsStackAvailabilityForSpecifiedItem() { }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsAvailable_ReturnsItemAvailabilityForSpecifiedItem(bool storeItemBeforeTest, bool expectedAvailable)
        {
            var inv = new Inventory<InventoryItemBase>(4);
            var item = new InventoryItemBase() { SlotsRequired = 1 };

            if(storeItemBeforeTest)
                inv.Store(item);

            Assert.AreEqual(expectedAvailable, inv.IsAvailable(item));
        }

        [Test] // TODO IsAvailable_ReturnsItemAvailabilityForSpecifiedItemStackSize
        public void IsAvailable_ReturnsItemAvailabilityForSpecifiedItemStackSize() { }

        [Test]
        [TestCase(4, 1, 3)]
        [TestCase(4, 4, 0)]
        public void FreeSlots_ReturnsTotalSlotsMinusSlotsUsed(int numberOfSlots, int itemRequiredSlots, int expectedFreeSlots)
        {
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            var item = new InventoryItemBase() { SlotsRequired = itemRequiredSlots };

            inv.Store(item);

            Assert.AreEqual(expectedFreeSlots, inv.FreeSlots);
        }

        [Test]
        [TestCase(4, 1)]
        [TestCase(4, 4)]
        public void UsedSlots_ReturnsSlotsUsed(int numberOfSlots, int itemRequiredSlots)
        {
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            var item = new InventoryItemBase() { SlotsRequired = itemRequiredSlots };

            inv.Store(item);

            Assert.AreEqual(itemRequiredSlots, inv.UsedSlots);
        }

        [Test]
        [TestCase(4, 1, 2, true)]
        [TestCase(4, 1, 6, false)]
        public void Store_MakesItemAvailable_IfHasRoom(int numberOfSlots, int itemStackSize, int itemRequiredSlots, bool expectedIsAvailable)
        {
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            var item = new InventoryItemBase()
            {
                ID = 42,
                StackSize = itemStackSize,
                SlotsRequired = itemRequiredSlots,
            };

            try { inv.Store(item); }
            catch (InventoryException) { }

            Assert.AreEqual(expectedIsAvailable, inv.IsAvailable(item));
        }

        [Test]
        [TestCase(4, 1, 6)]
        public void Store_ThrowsException_IfNotHasRoom(int numberOfSlots, int itemStackSize, int itemRequiredSlots)
        {
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            var item = new InventoryItemBase()
            {
                ID = 42,
                StackSize = itemStackSize,
                SlotsRequired = itemRequiredSlots,
            };

            Assert.Throws<InventoryException>(() => { inv.Store(item); });
        }

        [Test]
        [TestCase(2, 2, false)]
        [TestCase(2, 3, true)]
        public void Drain_RemovesAndReturnsItem_IfAvailable(int itemsToStore, int itemsToDrain, bool expectException)
        {
            int numberOfSlots = 4;
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);
            var item = new InventoryItemBase()
            {
                ID = 42,
                StackSize = 1,
                SlotsRequired = 1,
            };

            for(int i = 0; i < itemsToStore; i++)
                inv.Store(item);

            for(int i = 0; i < itemsToDrain; i++)
            {
                InventoryItemBase drainedItem = null;

                if (i >= itemsToStore && expectException)
                    Assert.Throws<InventoryException>(() => { drainedItem = inv.Drain(item); });
                else
                    drainedItem = inv.Drain(item);

                if (i < itemsToStore)
                    Assert.IsNotNull(drainedItem);
                else
                    Assert.IsNull(drainedItem);
            }

        }

        [Test] // TODO Drain_RemovesAndReturnsItem_IfStacksAvailable
        public void Drain_RemovesAndReturnsItem_IfStacksAvailable() { }

        [Test] // TODO AutoStack_StacksItemsOfSameTypeInMaxStackSizes
        public void AutoStack_StacksItemsOfSameTypeInMaxStackSizes() { }
    }
}
