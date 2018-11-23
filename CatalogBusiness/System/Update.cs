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

        /*  NAME:   updateSystem
         *  DESC:   Converts a CMOSystem to a Description and then passes it to the CatalogFacade
         *          for an update.
         *  PARAM:  {0} cmo - the CMOSystem to update with all required fields supplied
         *  RETURN: The same CMOSystem that was passed, but with updated status
         */
        public CMOSystem updateSystem(CMOSystem cmo)
        {
            try
            {
                // check for required parameters
                string missingField = "";
                if (cmo.SystemId == 0)
                    missingField = "SystemId";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMO is missing the required field '{0}'.", missingField));

                // convert to description
                SystemCatalogDescription desc = new SystemCatalogDescription()
                {
                    Description = cmo.Description,
                    ID = cmo.SystemId,
                    IsUII = cmo.IsUII,
                    UIICode = cmo.UIICode
                };

                // update system
                var result = cf.UpdateCatalogSystem(desc);

                // set response
                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format(""), ex);

                cmo.StatusCode = ((CMException)ex).ErrorCode;
                cmo.StatusDetail = ex.Message;
            }

            return cmo;
        }

    }
}
