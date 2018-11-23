using CatalogData.Exceptions;
using System;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Text;
using Catalog.Facade.Wrappers;

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
        public WPRMaterialCategoryDescription UpdateCatalogMaterialCategory(MaterialCategoryDescription matCatUpdates)
        {
            WPRMaterialCategoryDescription Return = WPRMaterialCategoryDescription.Wrap(matCatUpdates);

            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCatalogMaterialCategory(matCatUpdates);

                if(result == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update the Material Category: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested Material Category [ID: {0}]: {1}", matCatUpdates.ID, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
