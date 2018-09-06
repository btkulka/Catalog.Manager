using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.Components
{
    public partial class CatalogFacade
    {

        /*  NAME:   DeleteCatalogComponent
         *  DESC:   Deletes the given Catalog Component from the BUILDER database.
         *  PARAMS: {0} componentId - the Id of the Component to be deleted.
         *  RETURN: The newly deleted Catalog Component
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void DeleteCatalogSystem(int componentId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the requested Component [ID: {0}]: {1}", componentId, ex.Message), ex);
            }
        }
    }
}
