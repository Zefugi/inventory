using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Zefugi.Inventory.Tests
{
    [TestFixture]
    public class RepositorySystem_Tests
    {
        [Test]
        public void Add_MakesItemInfoAvailable()
        {
            var repo = new RepositorySystem();

            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;

            repo.Add(itemA);
            repo.Add(itemB);

            Assert.AreEqual(itemA, repo[itemA.ID]);
            Assert.AreEqual(itemB, repo[itemB.ID]);
        }

        [Test]
        public void Add_OverwritesItem_IfIdExists()
        {
            var sharedID = 1;
            var repo = new RepositorySystem();

            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = sharedID;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = sharedID;

            repo.Add(itemA);
            repo.Add(itemB);

            Assert.AreEqual(itemB, repo[sharedID]);
        }

        [Test]
        public void AddRange_AddsOrOverwritesFullList()
        {
            var sharedID = 1;
            var uniqueID = 2;
            var repo = new RepositorySystem();

            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = sharedID;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = sharedID;
            var itemC = Substitute.For<IItemInfo>();
            itemC.ID = uniqueID;

            repo.Add(itemA);
            repo.Add(itemB);
            repo.Add(itemC);

            Assert.AreEqual(itemB, repo[sharedID]);
            Assert.AreEqual(itemC, repo[uniqueID]);
        }

        [Test]
        public void Clear_RemovesAllItems()
        {
            var repo = new RepositorySystem();

            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;

            repo.Add(itemA);
            repo.Add(itemB);
            repo.Clear();

            Assert.AreEqual(0, repo.Count);
        }

        [Test]
        public void Count_ReturnsNumberOfItems()
        {
            var repo = new RepositorySystem();

            var itemA = Substitute.For<IItemInfo>();
            itemA.ID = 1;
            var itemB = Substitute.For<IItemInfo>();
            itemB.ID = 2;

            repo.Add(itemA);
            Assert.AreEqual(1, repo.Count);
            repo.Add(itemB);
            Assert.AreEqual(2, repo.Count);
            repo.Clear();
            Assert.AreEqual(0, repo.Count);
        }

        [Test] // TODO Indexer_ReturnsItem_IfAvailable_Else_ReturnsNull
        public void Indexer_ReturnsItem_IfAvailable_Else_ReturnsNull() { }
    }
}
