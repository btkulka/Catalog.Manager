using Catalog.Facade.BUILDERCatalog;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogSubComponent
         *  DESC:   Deletes the Catalog SubComponent with the associated Id from the BUILDER database.
         *  PARAMS: {0} subCompId - the sub component Id
         *  RETURN: A boolean indicating if the call was successful.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public bool DeleteCatalogSubComponent(int subCompId)
        {
            try
            {
                var cc = InitCatalogClient();
                var result = cc.DeleteCatalogSubComponent(subCompId);

                if(result == FunctionResultMessage.Success)
                {
                    return true;
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to delete SubComponent [ID: {0}]: {1}", subCompId, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);

                return false;
            }
        }
    }
}
