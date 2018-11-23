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
    public class ComponentTypeTests : TestingClass
    {

        public void GetAllComponentTypes_TestSuccess()
        {
            // Test getting all ComponentTypes
            var results = cf.GetCatalogComponentTypes();

            // Should get at least one record
            Assert.IsTrue(results.Count() > 0);
        }

        [Test]
        public void GetComponentType_TestFail()
        {
            // Pass invalid Id
            var results = cf.GetCatalogComponentType(-1);

            Assert.IsTrue(results.StatusCode != "200");
        }

        [Test]
        public void GetComponentType_TestSuccess()
        {
            // Create a valid ComponentType
            ComponentTypeDescription desc = new ComponentTypeDescription()
            {

            };

            // create the component
            cf.CreateCatalogComponentType(desc);

            // Fetch the newly created component
            var result = cf.GetCatalogComponentType(desc.ID);

            Assert.Equals(desc.Description, result.Description);
        }

        [Test]
        public void CreateComponentType_TestFail()
        {
            // Create an invalid description
            ComponentTypeDescription desc = new ComponentTypeDescription()
            {
                ID = -999
            };

            // Create the Component Type
            var result = cf.CreateCatalogComponentType(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void CreateComponentType_TestSuccess()
        {
            // Create a valid ComponentType
            ComponentTypeDescription desc = new ComponentTypeDescription()
            {

            };

            // create the component
            var result = cf.CreateCatalogComponentType(desc);

            Assert.IsTrue(result.StatusCode == "200");
        }

        [Test]
        public void DeleteComponentType_TestFail()
        {
            // pass an invalid id
            var result = cf.DeleteCatalogComponentType(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteComponentType_TestSuccess()
        {
            // create valid component
            ComponentTypeDescription desc = new ComponentTypeDescription()
            {
                Description = "TEST COMPONENT TYPE",
                ID = 10
            };
            
            // Create the component type
            cf.CreateCatalogComponentType(desc);

            // Delete the newly created component type
            var result = cf.DeleteCatalogComponentType(10);

            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateComponentType_TestSuccess()
        {

        }

        [Test]
        public void UpdateComponentType_TestFail()
        {

        }
    }
}
