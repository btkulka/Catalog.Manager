using CatalogData.Exceptions;
using System;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   UpdateCatalogMaterialCategory
         *  DESC:   Updates a Catalog Material Category from the BUILDER database.
         *  PARAMS: {0} matCatUpdates - an object holding all fields to update
         *  RETURN: The newly update Catalog Material Cateogry
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: add reference for MaterialCategoryDescription
        public void UpdateCatalogMaterialCategory(MaterialCategoryDescription matCatUpdates)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested Material Category [ID: {0}]: {1}", matCatUpdates.ID, ex.Message), ex);
            }
        }
    }
}
