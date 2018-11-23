using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogFacade;
using CatalogData.Classes;

namespace CatalogBusiness
{
    public partial class CatalogBusiness
    {
        // A reference to the CatalogFacade
        private CatalogFacade.CatalogFacade cf;

        // A constructor for the CatalogBusiness class
        public CatalogBusiness(CatalogCredentials cred)
        {
            cf = new CatalogFacade.CatalogFacade(cred);
        }

    }
}
