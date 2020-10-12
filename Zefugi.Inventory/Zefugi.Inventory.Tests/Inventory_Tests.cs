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
        public void Ctor_SetsNumberOfSlots() { }

        [Test]
        public void SlotsTotal_GetsNumberOfSlotsInTotal() { }

        [Test]
        public void SlotsTotal_SetThrowsException_IfSlotsInUse() { }

        [Test]
        public void HasRoom_ReturnsSlotAvailabilityForSpecifiedItem() { }

        [Test]
        public void IsAvailable_ReturnsItemAvailabilityForSpecifiedItem() { }

        [Test]
        public void GetFreeSlots_ReturnsTotalSlotsMinusSlotsUsed() { }

        [Test]
        public void GetUsedSlots_ReturnsSlotsUsed() { }

        [Test]
        public void Store_MakesItemAvailable_IfHasRoom() { }

        [Test]
        public void Drain_RemovesAndReturnsItem_IfAvailable() { }

        [Test]
        public void AutoStack_StacksItemsOfSameTypeInMaxStackSizes() { }
    }
}
