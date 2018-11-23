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
        /*  NAME:   updateSubComponent
         *  DESC:   Converts a CMOSubComponent to a Description and then passes it to the CatalogFacade
         *          for an update.
         *  PARAM:  {0} cmo - the CMOSubComponent to update with all required fields supplied
         *  RETURN: The same CMOSubComponent that was passed, but with updated status
         */
        public CMOSubComponent updateSubComponent(CMOSubComponent cmo)
        {
            try
            {
                // Check for required parameters
                string missingField = "";
                if (cmo.SubComponentId == 0)
                    missingField = "SubComponentId";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMO is missing required field '{0}'.", missingField));

                // Convert to description
                SubComponentDescription desc = new SubComponentDescription()
                {
                    CompUnitsID = cmo.CompUnitsId,
                    Description = cmo.Description,
                    ID = cmo.SubComponentId
                };

                // Update SubComponent
                var result = cf.UpdateCatalogSubComponent(desc);

                // Set response
                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while trying to update this CMO: {0}", ex.Message), ex);

                cmo.StatusCode = ((CMException)ex).ErrorCode;
                cmo.StatusDetail = ex.Message;
            }
            return cmo;
        }

    }
}
