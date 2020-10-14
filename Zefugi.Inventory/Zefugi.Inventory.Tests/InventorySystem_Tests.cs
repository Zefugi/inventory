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

        [Test] // TODO TotalSlots_CanShrink_IfSpaceAvailable
        public void TotalSlots_CanShrink_IfSpaceAvailable() { }

        [Test] // TODO TotalSlots_Throws_WhenShrink_IfSpaceUnavailable
        public void TotalSlots_Throws_WhenShrink_IfSpaceUnavailable() { }

        [Test] // TODO Store_MakesItemAvailable_IfSpaceAvailable
        public void Add_MakesItemAvailable_IfSpaceAvailable() { }

        [Test] // TODO Store_Throws_IfSpaceUnavailable
        public void Store_Throws_IfSpaceUnavailable() { }

        [Test] // TODO Store_Compresses_IfAutoStackIsTrue
        public void Store_Compresses_IfAutoStackIsTrue() { }

        [Test] // TODO Retrieve_ReturnsItemAndMakesItemUnavailable_IfItemIsAvailable
        public void Retrieve_ReturnsItemAndMakesItemUnavailable_IfItemIsAvailable() { }

        [Test] // TODO Retrieve_Throws_ifItemIsUnavailable
        public void Retrieve_Throws_ifItemIsUnavailable() { }

        [Test] // TODO Retrieve_Compresses_IfAutoStackIsTrue
        public void Retrieve_Compresses_IfAutoStackIsTrue() { }

        [Test] // TODO UsedSlots_ReturnsUsedSlots
        public void UsedSlots_ReturnsUsedSlots() { }

        [Test] // TODO FreeSlots_ReturnsUnusedSlots
        public void FreeSlots_ReturnsUnusedSlots() { }

        [Test] // TODO Clear_ReturnsAllItemsAndMakesThemUnavailable
        public void Clear_ReturnsAllItemsAndMakesThemUnavailable() { }

        [Test] // TODO IsAvailable_ReturnsTrue_OnlyIfTheSpecifiedItemsAreAvailable
        public void IsAvailable_ReturnsTrue_OnlyIfTheSpecifiedItemsAreAvailable() { }

        [Test] // TODO AutoStack_StacksItemsToSaveSlots
        public void Compress_StacksItemsToSaveSlots() { }
    }
}
