using Catalog.Facade.BUILDERCatalog;
using Catalog.Facade.Wrappers;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogFacade
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
        public WPRMaterialCategoryDescription GetCatalogMaterialCategory(int matCatId)
        {
            WPRMaterialCategoryDescription retval = new WPRMaterialCategoryDescription();
            retval.ID = matCatId;

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogMaterialCategory(matCatId);

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = (WPRMaterialCategoryDescription)new List<MaterialCategoryDescription>(result.MatCatCatalog).FirstOrDefault();
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to fetch the requested Material Category [ID: {0}]: {1}", matCatId, result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested Material Category [ID: {0}]: {1}", matCatId, ex.Message), ex);
                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }

        /*  NAME:   GetCatalogMaterialCategories
         *  DESC:   Fetches all available Catalog Material Categories
         *  RETURN: A list of all available Catalog Material Categories
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<MaterialCategoryDescription> GetCatalogMaterialCategories()
        {
            List<MaterialCategoryDescription> retval = new List<MaterialCategoryDescription>();

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogMaterialCategories();

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = new List<MaterialCategoryDescription>(result.MatCatCatalog);
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve all Catalog Material Categories: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all available Material Categories: {0}", ex.Message), ex);
            }

            return retval;
        }
    }
}
