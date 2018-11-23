using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogSystem
         *  DESC:   Deletes the given Catalog System from the BUILDER database.
         *  PARAMS: {0} systemId - the Id of the System to be deleted.
         *  RETURN: A boolean indicating if the call was successful.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public bool DeleteCatalogSystem(int systemId)
        {
            try
            {
                var cc = InitCatalogClient();
                var result = cc.DeleteCatalogSystem(systemId);

                if(result == Catalog.Facade.BUILDERCatalog.FunctionResultMessage.Success)
                {
                    return true;
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to delete Catalog System [ID: {0}]: {1}", systemId, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested System [ID: {0}]: {1}", systemId, ex.Message), ex);

                return false;
            }
        }
    }
}
