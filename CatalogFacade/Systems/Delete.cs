using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.Systems
{
    public partial class CatalogFacade
    {
        /*  NAME:   DeleteCatalogSystem
         *  DESC:   Deletes the given Catalog System from the BUILDER database.
         *  PARAMS: {0} systemId - the Id of the System to be deleted.
         *  RETURN: The newly deleted Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void DeleteCatalogSystem(int systemId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested System [ID: {0}]: {1}", systemId, ex.Message), ex);
            }
        }
    }
}
