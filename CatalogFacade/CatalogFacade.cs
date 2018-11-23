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

        public CatalogFacade(CatalogCredentials c)
        {
            this.cred = c;
        }

        public Catalog.Facade.BUILDERCatalog.CatalogClient InitCatalogClient()
        {
            Binding binding = new WSHttpBinding();
            CatalogClient cc = new Catalog.Facade.BUILDERCatalog.CatalogClient("wsHttpBinding", cred.RemoteAddress);
            cc.ClientCredentials.UserName.UserName = cred.BUILDERUsername;
            cc.ClientCredentials.UserName.Password = cred.BUILDERPassword;

            return cc;
        }
    }
}
