using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.MaterialCategories
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogMaterialCategory
         *  DESC:   Fetches the Catalog Material Category with the given id.
         *  PARAMS: {0} matCatId - the material category id
         *  RETURN: The fetched Catalog Material Category
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogMaterialCategory(int matCatId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested Material Category [ID: {0}]: {1}", matCatId, ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogMaterialCategories
         *  DESC:   Fetches all available Catalog Material Categories
         *  RETURN: A list of all available Catalog Material Categories
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogMaterialCategories()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all available Material Categories: {0}", ex.Message), ex);
            }
        }
    }
}
