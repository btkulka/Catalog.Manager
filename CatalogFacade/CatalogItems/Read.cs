using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetAllCatalogItems
         *  DESC:   Returns all available Catalog Items.
         *  RETURN: A list of all available Catalog items.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetAllCatalogItems()
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Catalog Items: {0}", ex.Message), ex);
            }
        }

        /*  NAME:   GetCatalogItem
         *  DESC:   Fetches the item associated with the cmcId
         *  PARAMS: {0} cmcId - the catalog item id
         *  RETURN: The requested CatalogItem
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void GetCatalogItem(int cmcId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch Catalog Item [Id: {0}]: {1}", cmcId, ex.Message), ex);
            }
        }
    }
}
