using Catalog.Facade.BUILDERCatalog;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogComponentType
         *  DESC:   Deletes the given Catalog Component Type from the BUILDER database.
         *  PARAMS: {0} compTypeId - the Id of the Component Type to be deleted.
         *  RETURN: A boolean indicating if the call was successful
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public bool DeleteCatalogComponentType(int compTypeId)
        {
            try
            {
                var cc = InitCatalogClient();
                var result = cc.DeleteCatalogComponentType(compTypeId);

                if(result == FunctionResultMessage.Success)
                {
                    return true;
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to delete Component Type [ID: {0}]: {1}", compTypeId, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the given Component Type [Id: {0}]: {1}", compTypeId, ex.Message), ex);

                return false;
            }
        }
    }
}
