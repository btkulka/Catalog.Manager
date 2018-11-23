using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes.CMOs;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogBusiness
{
    public partial class CatalogBusiness
    {

        /*  NAME:   createCatalogComponent
         *  DESC:   Takes a CMOComponent, checks that it is formatted correctly, then converts it to a Description and sends to the CatalogFacade.
         *  PARAM:  {0} comp - the new CMOComponent to be added to the BUILDER Catalog.
         *  RETURN: The same comp CMO with updated Status Code and Details.
         */
        public CMOComponent createCatalogComponent(CMOComponent comp)
        {
            try
            {
                // Check for required fields
                string missingField = "";

                // Throw exception if missing field exists
                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("Object is missing the required field '{0}'.", missingField));

                // Convert to description
                ComponentCatalogDescription desc = new ComponentCatalogDescription()
                {
                    Description = comp.Description,
                    ID = comp.ComponentId,
                    IsEquip = comp.IsEquip,
                    IsUII = comp.IsUII,
                    SysId = comp.SystemId,
                    UIICode = comp.UIICode
                };

                var result = cf.CreateCatalogComponents(desc);

                comp.StatusCode = result.StatusCode;
                comp.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while attempting to create Catalog Component: {0}", ex.Message), ex);
            }

            return comp;
        }

    }
}
