using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.SubComponents
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogSubComponents
         *  DESC:   Fetches all available Catalog SubComponents
         *  RETURN: A list of all available Catalog SubComponents
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogSubComponents()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all SubComponents: {0}", ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogSubComponent
         *  DESC:   Fetches the Catalog SubComponent with the given id.
         *  PARAMS: {0} subCompId - the SubComponent id
         *  RETURN: The fetched Catalog SubComponent
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogSubComponent(int subCompId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);
            }
        }
    }
}
