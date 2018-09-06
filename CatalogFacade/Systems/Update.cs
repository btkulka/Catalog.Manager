using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   UpdateCatalogSystem
         *  DESC:   Updates the given Catalog System.
         *  PARAMS: {0} sysUpdates - a SystemCatalogDescription object with all fields to update
         *  RETURN: The newly updated Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: add reference for SystemCatalogDescription
        public void UpdateCatalogSystem(SystemCatalogDescription sysUpdates)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested System [ID: {0}]: {1}", sysUpdates.ID, ex.Message), ex);
            }
        }
    }
}
