using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   UpdateCatalogSubComponent
         *  DESC:   Updates a Catalog SubComponent from the BUILDER database.
         *  PARAMS: {0} subCompUpdates - an object holding all fields to update
         *  RETURN: The newly updated Catalog SubComponent
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: Add reference for SubComponentDescription
        public void UpdateCatalogSubComponent(SubComponentDescription subCompUpdates)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested SubComponent [ID: {0}]: {1}", subCompUpdates.ID, ex.Message), ex);
            }
        }
    }
}
