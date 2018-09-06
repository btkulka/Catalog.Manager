using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.ComponentType
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogComponentType
         *  DESC:   Fetches the given Catalog Component Type from the BUILDER database.
         *  PARAMS: {0} compTypeId - the Id of the Component Type to be fetched.
         *  RETURN: The fetched Catalog Component Type
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogComponentType(int compTypeId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the given Component Type [Id: {0}]: {1}", compTypeId, ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogComponentTypes
         *  DESC:   Returns all available Component Types.
         *  RETURN: A list of all Catalog Component Types
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogComponentTypes()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Component Types: {0}", ex.Message), ex);
            }
        }
    }
}
