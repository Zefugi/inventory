using NUnit.Framework;
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

        [Test] // TODO
        public void SlotsTotal_SetThrowsException_IfSlotsInUse() { }

        [Test]
        [TestCase(1, true)]
        [TestCase(5, false)]
        public void HasRoom_ReturnsSlotAvailabilityForSpecifiedItem(int itemSlotsRequired, bool expectedHasRoom)
        {
            int numberOfSlots = 4;
            var inv = new Inventory<InventoryItemBase>(numberOfSlots);

            bool hasRoom = inv.HasRoom(new InventoryItemBase()
            {
                SlotsRequired = itemSlotsRequired,
            });

            Assert.AreEqual(expectedHasRoom, hasRoom);
        }

        [Test] // TODO
        public void HasRoom_ReturnsStackAvailabilityForSpecifiedItem() { }

        [Test] // TODO
        public void IsAvailable_ReturnsItemAvailabilityForSpecifiedItem() { }

        [Test] // TODO
        public void FreeSlots_ReturnsTotalSlotsMinusSlotsUsed() { }

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

        [Test] // TODO
        public void Drain_RemovesAndReturnsItem_IfAvailable() { }

        [Test] // TODO
        public void AutoStack_StacksItemsOfSameTypeInMaxStackSizes() { }
    }
}
