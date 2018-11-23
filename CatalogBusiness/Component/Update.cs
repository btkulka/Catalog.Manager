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

        /*  NAME:   updateComponent
         *  DESC:   Takes a CMO, converts it to a description, then passes it to the Facade to update
         *  PARAM:  {0} cmo - a CMOComponent with all required fields
         *  RETURN: The same CMOComponent with updated Status.
         */
        public CMOComponent updateComponent(CMOComponent cmo)
        {
            try
            {
                // Check for required fields
                string missingField = "";
                if (cmo.ComponentId == null)
                    missingField = "ComponentId";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMO Component is missing the required field '{0}'.", missingField));

                // Convert to Description
                ComponentCatalogDescription desc = new ComponentCatalogDescription()
                {
                    Description = cmo.Description,
                    ID = cmo.ComponentId,
                    IsEquip = cmo.IsEquip,
                    IsUII = cmo.IsUII,
                    SysId = cmo.SystemId,
                    UIICode = cmo.UIICode
                };

                // Update Component
                var result = cf.UpdateCatalogComponent(desc);

                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException();

                cmo.StatusCode = ((CMException)ex).ErrorCode;
                cmo.StatusDetail = ex.Message;
            }

            return cmo;
        }

    }
}
