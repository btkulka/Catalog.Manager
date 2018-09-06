using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.MaterialCategories
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogMaterialCategory
         *  DESC:   Deletes a new Catalog Material Category from the BUILDER database.
         *  PARAMS: {0} matCatId - Id of the material category
         *  RETURN: The newly deleted Material Category
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void DeleteCatalogMaterialCategory(int matCatId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested Material Category [ID: {0}]: {1}", matCatId, ex.Message), ex);
            }
        }
    }
}
