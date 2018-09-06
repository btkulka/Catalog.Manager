using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   UpdateCatalogComponentType
         *  DESC:   Updates the given Catalog Component Type
         *  PARAMS: {0} compTypeUpdates - an object with all updatable fields for Component Type
         *  RETURN: The newly deleted Catalog Component Type
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: Add reference for ComponentTypeDescription
        public void UpdateCatalogComponentType(ComponentTypeDescription compTypeUpdates)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the given Component Type [Id: {0}]: {1}", compTypeUpdates.ID, ex.Message), ex);
            }
        }
    }
}
