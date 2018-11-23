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
        /*  NAME:   updateComponentType
         *  DESC:   Takes a CMO, converts it to a description, then passes it to the Facade to update
         *  PARAM:  {0} cmo - a CMOComponentType with all required fields
         *  RETURN: The same CMOComponentType with updated Status.
         */
        public CMOComponentType updateComponentType(CMOComponentType cmo)
        {
            try
            {
                // Check for required fields
                string missingField = "";

                if (cmo.ComponentTypeId == 0)
                    missingField = "ComponentTypeId";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMO Component Type missing required field '{0}'.", missingField));

                // Convert to description
                ComponentTypeDescription desc = new ComponentTypeDescription()
                {
                    Description = cmo.Description,
                    ID = cmo.ComponentTypeId
                };

                // Update ComponentType
                var result = cf.UpdateCatalogComponentType(desc);

                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while attempting to update a Component Type: {0}", ex.Message), ex);

                cmo.StatusCode = ((CMException)ex).ErrorCode;
                cmo.StatusDetail = ex.Message;
            }

            return cmo;
        }

    }
}
