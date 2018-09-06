using System;
using CatalogData.Exceptions;

namespace CatalogManager
{
    public partial class CatalogFacade
    {

        /*  NAME:   GetCatalogSystem
         *  DESC:   Returns a specific Catalog System given SystemId
         *  PARAMS: {0} systemId - a valid Id representing a Catalog System.
         *  RETURN: The requested Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogSystem(int systemId)
        {
            try
            {

            }catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested System [ID: {0}]: {1}", systemId, ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogSystems
         *  DESC:   Returns all available Catalog Systems.
         *  RETURN: A list of all available Catalog Systems
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogSystems()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Catalog Systems: {0}", ex.Message), ex);
            }
        }

    }
}
