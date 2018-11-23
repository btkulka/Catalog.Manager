using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogTester.CatalogFacadeTests
{
    [TestFixture]
    public class CatalogItemTests : TestingClass
    {

        [Test]
        public void CreateCatalogItem_TestFail_NoParams()
        {
            CMCDescription desc = new CMCDescription()
            {

            };

            var result = cf.CreateCatalogItem(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void CreateCatalogItem_TestSuccess()
        {
            CMCDescription desc = new CMCDescription()
            {
                ID = 1,
                CompLink = 2043,
                MCatLink = 2058,
                CTypeLink = 5151,
                UoM = 1,
                CompUIILink = 2043,
                AdjFactor = 0.3f,
                CII = 0.5f
            };

            // make call
            var result = cf.CreateCatalogItem(desc);

            Assert.IsTrue(result.StatusCode == "200");

            // clear new item
            cf.DeleteCatalogItem(1);
        }

        [Test]
        public void DeleteCatalogItem_TestSuccess()
        {
            // Create a valid CatalogItem
            CMCDescription desc = new CMCDescription()
            {

            };

            // Create the item
            cf.CreateCatalogItem(desc);

            // Delete the new CatalogItem
            var result = cf.DeleteCatalogItem(desc.ID);

            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteCatalogItem_TestFail_InvalidID()
        {
            // Pass an invalid ID to delete
            var result = cf.DeleteCatalogItem(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void GetAllCatalogItems_TestSuccess()
        {

            // Retrieve all Catalog Items
            var result = cf.GetAllCatalogItems();

            // NOTE: This will fail on an empty Catalog.
            Assert.IsTrue(result.Count() > 0);
        }

        [Test]
        public void GetCatalogItem_TestFail_InvalidID()
        {
            // Pass an invalid ID to fetch
            var result = cf.GetCatalogItem(-1);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void GetCatalogItem_TestSuccess()
        {
            // Create a valid CatalogItem to fetch
            CMCDescription desc = new CMCDescription()
            {

            };

            // Create catalog item
            desc = cf.CreateCatalogItem(desc);

            // Fetch newly created CatalogItem
            var result = cf.GetCatalogItem(desc.ID);

            Assert.IsTrue(result.StatusCode == "200");
        }

        [Test]
        public void UpdateCatalogItem_TestFail_InvalidID()
        {
            // Has Invalid ID
            CMCDescription desc = new CMCDescription()
            {
                ID = -1,
                AdjFactor = 3,
                MCatLink = 4
            };

            var result = cf.UpdateCatalogItem(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void UpdateCatalogItem_TestSuccess()
        {
            // Valid CMCDescription
            CMCDescription desc = new CMCDescription()
            {

            };

            // Create the description
            cf.CreateCatalogItem(desc);

            // Change one field
            desc.MCatLink = 200;

            // Update
            cf.UpdateCatalogItem(desc);

            // FetchItem
            var result = cf.GetCatalogItem(desc.ID);

            Assert.Equals(result.MCatLink, desc.MCatLink);
        }
    }
}
