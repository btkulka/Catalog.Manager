using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   UpdateCatalogComponent
         *  DESC:   Updates the given Catalog Component.
         *  PARAMS: {0} compUpdates - a ComponentCatalogDescription object with all fields to update
         *  RETURN: The newly updated Catalog Component.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: add reference for ComponentCatalogDescription
        public void UpdateCatalogComponent(ComponentCatalogDescription compUpdates)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested System [ID: {0}]: {1}", compUpdates.ID, ex.Message), ex);
            }
        }

    }
}
