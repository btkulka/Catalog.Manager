using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogFacade;
using CatalogFacade.CatalogItems;

namespace CatalogTester.CatalogFacadeTests
{
    [TestClass]
    public class CatalogItems
    {
        private static CatalogFacade.CatalogFacade cf;

        /*  NAME:   CatalogFacadeTestingSuite
         *  DESC:   The master function that calls all other unit tests for CatalogItems.
         *          The whole suite is ran in one function, and results are output to (?).
         */ 
        public static void CatalogItemTestingSuite()
        {
            // Set up testing suite
            cf = new CatalogFacade.CatalogFacade();

            // Create new Catalog Items
            
            // Fetch newly created Catalog Items

            // Update those Catalog Items

            // Delete those Catalog Items
        }

        [TestMethod]
        public void TestGetCatalogItems()
        {

            
            
        }
    }
}
