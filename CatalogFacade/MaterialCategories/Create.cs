using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.MaterialCategories
{
    public partial class CatalogFacade
    {

        /*  NAME:   CreateCatalogMaterialCategory
         *  DESC:   Creates a new Catalog Material Category from the BUILDER database.
         *  PARAMS: {0} description - description of the material category
         *  RETURN: The newly created Catalog Material Cateogry
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogMaterialCategory(string description)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested Material Category [ID: {0}]: {1}", description, ex.Message), ex);
            }
        }

    }
}
