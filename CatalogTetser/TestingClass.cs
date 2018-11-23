using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogTester
{
    [TestClass()]
    public class TestingClass
    {
        public static string REMOTE_ADDRESS = "https://dev.buildersoftware.net/webservice/CatalogManager.svc";
        public static string BUILDER_UN = "ben@digonsystems.com";
        public static string BUILDER_PW = "DSpass4B";

        public CatalogFacade.CatalogFacade cf = new CatalogFacade.CatalogFacade(new CatalogCredentials(REMOTE_ADDRESS, BUILDER_UN, BUILDER_PW));

    }
}
