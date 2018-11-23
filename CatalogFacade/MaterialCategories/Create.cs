using Catalog.Facade.BUILDERCatalog;
using Catalog.Facade.Wrappers;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   CreateCatalogMaterialCategory
         *  DESC:   Creates a new Catalog Material Category from the BUILDER database.
         *  PARAMS: {0} desc - a MaterialCategoryDescription object with all required fields populated
         *  RETURN: The newly created Catalog Material Cateogry in a wrapper to communicate Status.
        */
        public WPRMaterialCategoryDescription CreateCatalogMaterialCategory(MaterialCategoryDescription desc)
        {
            WPRMaterialCategoryDescription Return = new WPRMaterialCategoryDescription();
            Return.ID = desc.ID;
            Return.CatDesc = desc.CatDesc;

            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCatalogMaterialCategory(desc.ID, desc.CatDesc);

                if(result.Status == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBusinessException(String.Format("BUILDER was unable to create a Material Category: {0}.", desc.CatDesc)); ;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested Material Category [ID: {0}]: {1}", desc.CatDesc, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }

    }
}
