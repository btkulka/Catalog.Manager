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
    public class ComponentTests : TestingClass
    {

        [Test]
        public void GetAllComponents_TestSuccess()
        {
            // Get all Components
            var results = cf.GetCatalogComponents();

            Assert.IsTrue(results.Count() > 0);
        }

        [Test]
        public void GetComponent_TestFail()
        {
            // Use an invalid ID
            var result = cf.GetCatalogComponent(-1);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void GetComponent_TestSuccess()
        {

        }

        [Test]
        public void CreateComponent_TestFail()
        {
            // Create invalid description
            ComponentCatalogDescription desc = new ComponentCatalogDescription()
            {
                ID = -1
            };

            // create the bad component
            var result = cf.CreateCatalogComponents(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void CreateComponent_TestSuccess()
        {

        }

        [Test]
        public void DeleteComponent_TestFail()
        {
            // pass an invalid ID
            var result = cf.DeleteCatalogComponent(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteComponent_TestSuccess()
        {

        }

        [Test]
        public void UpdateComponent_TestFail()
        {

        }

        [Test]
        public void UpdateComponent_TestSuccess()
        {

        }
    }
}
