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
    public class SystemTests : TestingClass
    {

        [Test]
        public void GetAllSystems_TestSuccess()
        {
            // make call
            var results = cf.GetCatalogSystems();

            Assert.IsTrue(results.Count() > 0);
        }

        [Test]
        public void GetSystem_TestFail()
        {
            // pass an invalid ID
            var result = cf.GetCatalogSystem(-1);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void GetSystem_TestSuccess()
        {
            var result = cf.GetCatalogSystem(201);

            Assert.IsTrue(result.StatusCode == "200");
        }

        [Test]
        public void CreateSystem_TestFail()
        {
            SystemCatalogDescription desc = new SystemCatalogDescription()
            {
               
            };

            var result = cf.CreateCatalogSystems(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void CreateSystem_TestSuccess()
        {
            SystemCatalogDescription desc = new SystemCatalogDescription()
            {
                ID = 1,
                Description = "B01 BEN TEST SYSTEM",
                UIICode = "B01",
                IsUII = true
            };

            var result = cf.CreateCatalogSystems(desc);

            Assert.IsTrue(result.StatusCode == "200");

            // clean up test record
            if(result.StatusCode == "200")
                cf.DeleteCatalogSystem(1);
        }

        [Test]
        public void UpdateSystem_TestFail()
        {

        }

        [Test]
        public void UpdateSystem_TestSuccess()
        {

        }

        [Test]
        public void DeleteSystem_TestFail()
        {
            // pass invalid id
            var result = cf.DeleteCatalogSystem(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteSystem_TestSuccess()
        {
            SystemCatalogDescription desc = new SystemCatalogDescription()
            {
                ID = 1,
                Description = "B01 BEN TEST SYSTEM",
                UIICode = "B01",
                IsUII = true
            };

            cf.CreateCatalogSystems(desc);

            var result = cf.DeleteCatalogSystem(1);

            Assert.IsTrue(result);
        }
    }
}
