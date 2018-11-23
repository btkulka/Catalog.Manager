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
        /*  NAME:   createComponentType
         *  DESC:   Takes a CMOComponentType, checks that it is formatted correctly, then converts it to a Description and sends to the CatalogFacade.
         *  PARAM:  {0} compType - the new CMOComponentType to be added to the BUILDER Catalog.
         *  RETURN: The same compType CMO with updated Status Code and Details.
         */
        public CMOComponentType createComponentType(CMOComponentType compType)
        {
            try
            {
                // Check for required fields
                string missingField = "";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("Object is missing required field '{0}'.", missingField));

                // Convert to description
                ComponentTypeDescription desc = new ComponentTypeDescription()
                {
                    Description = compType.Description,
                    ID = compType.ComponentTypeId
                };

                var result = cf.CreateCatalogComponentType(desc);

                compType.StatusCode = result.StatusCode;
                compType.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue when attempting to create a Component Type: {0}", ex.Message), ex);

                compType.StatusCode = ((CMException)ex).ErrorCode;
                compType.StatusDetail = ex.Message;
            }

            return compType;
        }

    }
}
