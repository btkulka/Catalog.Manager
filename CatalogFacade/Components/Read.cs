using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.Components
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogComponent
         *  DESC:   Returns a specific Catalog System given SystemId
         *  PARAMS: {0} componentId - a valid Id representing a Catalog Component.
         *  RETURN: The requested Catalog Component.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogComponent(int componentId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested Component [ID: {0}]: {1}", componentId, ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogComponents
         *  DESC:   Returns all available Catalog Components.
         *  RETURN: A list of all available Catalog Components.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogComponents()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Catalog Components: {0}", ex.Message), ex);
            }
        }
    }
}
