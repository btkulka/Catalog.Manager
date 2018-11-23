using Catalog.Facade.BUILDERCatalog;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogTester.CatalogFacadeTests
{
    [TestFixture]
    public class SubComponentTests : TestingClass
    {

        [Test]
        public void GetAllSubComponents()
        {
            // make call
            var results = cf.GetCatalogSubComponents();

            Assert.IsTrue(results.Count() > 0);
        }

        [Test]
        public void GetSubComponent_TestFail()
        {
            // pass invalid Id
            var results = cf.GetCatalogSubComponent(-1);

            Assert.IsTrue(results.StatusCode != "200");
        }

        [Test]
        public void GetSubComponnet_TestSuccess()
        {

        }

        [Test]
        public void CreateSubComponent_TestFail()
        {
            SubComponentDescription desc = new SubComponentDescription()
            {

            };

            var result = cf.CreateCatalogSubComponent(desc);

            Assert.IsTrue(result.StatusCode != "200");

            // delete in case test passes
            if (result.StatusCode == "200")
                cf.DeleteCatalogSubComponent(desc.ID);
        }

        [Test]
        public void CreateSubComponent_TestSuccess()
        {
            SubComponentDescription desc = new SubComponentDescription()
            {
                ID = 2,
                Description = "BEN TEST SUBCOMPONENT",
                CompUnitsID = 1
            };

            var result = cf.CreateCatalogSubComponent(desc);

            Assert.IsTrue(result.StatusCode == "200");

            // delete in case test passes
            if (result.StatusCode == "200")
                cf.DeleteCatalogSubComponent(desc.ID);
        }

        [Test]
        public void UpdateSubComponent_TestFail()
        {
            // subcomponent dne
            SubComponentDescription desc = new SubComponentDescription()
            {
                ID = -1,
                Description = "BEN TEST SUBCOMPONENT",
                CompUnitsID = 1
            };

            var result = cf.UpdateCatalogSubComponent(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void UpdateSubComponent_TestSuccess()
        {
            SubComponentDescription desc = new SubComponentDescription()
            {
                ID = 2,
                Description = "BEN TEST SUBCOMPONENT",
                CompUnitsID = 1
            };

            cf.CreateCatalogSubComponent(desc);

            desc.Description = "UPDATED";

            var result = cf.UpdateCatalogSubComponent(desc);

            Assert.IsTrue(result.StatusCode == "200" && result.Description == "UPDATED");

            // delete if test passes
            if (result.StatusCode == "200")
                cf.DeleteCatalogSubComponent(desc.ID);
        }

        [Test]
        public void DeleteSubComponent_TestFail()
        {
            var result = cf.DeleteCatalogSubComponent(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteSubComponent_TestSuccess()
        {
            SubComponentDescription desc = new SubComponentDescription()
            {
                ID = 2,
                Description = "BEN TEST SUBCOMPONENT",
                CompUnitsID = 1
            };

            cf.CreateCatalogSubComponent(desc);

            Assert.IsTrue(cf.DeleteCatalogSubComponent(desc.ID));
        }
    }
}
