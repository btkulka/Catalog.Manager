using System;
using System.Collections.Generic;
using System.Text;
using CatalogData.Classes;
using Catalog.Facade.BUILDERCatalog;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        public CatalogCredentials cred { get; set; }
        public Guid SessionId { get; set; }

        public Catalog.Facade.BUILDERCatalog.CatalogClient InitCatalogClient(CatalogCredentials cred)
        {
            Binding binding = new WSHttpBinding();
            CatalogClient cc = new Catalog.Facade.BUILDERCatalog.CatalogClient("", cred.RemoteAddress);
            return cc;
        }
    }
}
