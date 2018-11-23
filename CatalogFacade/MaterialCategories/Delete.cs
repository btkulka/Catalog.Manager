using Catalog.Facade.BUILDERCatalog;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogMaterialCategory
         *  DESC:   Deletes a new Catalog Material Category from the BUILDER database.
         *  PARAMS: {0} matCatId - Id of the material category
         *  RETURN: A bool indicating whether or not the delete was successful.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public bool DeleteCatalogMaterialCategory(int matCatId)
        {
            try
            {
                var cc = InitCatalogClient();
                var result = cc.DeleteCatalogMaterialCategory(matCatId);

                if(result == FunctionResultMessage.Success)
                {
                    return true;
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to delete Material Category [ID: {0}]: {1}", matCatId, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested Material Category [ID: {0}]: {1}", matCatId, ex.Message), ex);

                return false;
            }
        }
    }
}
