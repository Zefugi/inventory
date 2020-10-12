﻿using NUnit.Framework;
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
        public void HasRoom_ReturnsStackAvailabilityForSpecifiedItem(int itemStackSize, bool expectedHasRoom) { }

        [Test] // TODO
        public void IsAvailable_ReturnsItemAvailabilityForSpecifiedItem() { }

        [Test] // TODO
        public void GetFreeSlots_ReturnsTotalSlotsMinusSlotsUsed() { }

        [Test] // TODO
        public void GetUsedSlots_ReturnsSlotsUsed() { }

        [Test] // TODO
        public void Store_MakesItemAvailable_IfHasRoom() { }

        [Test] // TODO
        public void Drain_RemovesAndReturnsItem_IfAvailable() { }

        [Test] // TODO
        public void AutoStack_StacksItemsOfSameTypeInMaxStackSizes() { }
    }
}
