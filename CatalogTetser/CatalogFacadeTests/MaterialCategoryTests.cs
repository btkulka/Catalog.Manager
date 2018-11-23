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
    public class MaterialCategoryTests : TestingClass
    {

        [Test]
        public void GetAllMaterialCategories()
        {
            // Retrieve all Material Categories
            var results = cf.GetCatalogMaterialCategories();

            Assert.IsTrue(results.Count() > 0);
        }

        [Test]
        public void GetMaterialCategory_TestFail()
        {
            // Pass invalid ID for fetch
            var result = cf.GetCatalogMaterialCategory(-1);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void GetMaterialCategory_TestSuccess()
        {
            // Retrieve existing MaterialCategory
            var result = cf.GetCatalogMaterialCategory(2000);

            Assert.IsTrue(result.StatusCode == "200");
        }

        [Test]
        public void CreateMaterialCategory_TestFail()
        {
            // Create with an invalid ID
            MaterialCategoryDescription desc = new MaterialCategoryDescription()
            {
                ID = -1,
                CatDesc = "B101 BEN MATERIAL"
            };

            // create the material category
            var result = cf.CreateCatalogMaterialCategory(desc);

            Assert.IsTrue(result.StatusCode != "200");
        }

        [Test]
        public void CreateMaterialCategory_TestSuccess()
        {
            // Create with an invalid ID
            MaterialCategoryDescription desc = new MaterialCategoryDescription()
            {
                ID = 1,
                CatDesc = "B101 BEN MATERIAL"
            };

            // create the material category
            var result = cf.CreateCatalogMaterialCategory(desc);

            Assert.IsTrue(result.StatusCode == "200");

            // delete test material
            cf.DeleteCatalogMaterialCategory(1);
        }

        [Test]
        public void DeleteMaterialCategory_TestFail()
        {
            // create the material category
            var result = cf.DeleteCatalogMaterialCategory(-1);

            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteMaterialCategory_TestSuccess()
        {
            // Create with an invalid ID
            MaterialCategoryDescription desc = new MaterialCategoryDescription()
            {
                ID = 1,
                CatDesc = "B101 BEN MATERIAL"
            };

            // create the material category
            cf.CreateCatalogMaterialCategory(desc);

            // delete test material
            var result = cf.DeleteCatalogMaterialCategory(1);

            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateMaterialCategory_TestFail()
        {

        }

        [Test]
        public void UpdateMaterialCategory_TestSuccess()
        {

        }
    }
}
