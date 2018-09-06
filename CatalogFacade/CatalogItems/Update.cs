using Catalog.Facade.BUILDERCatalog;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade.CatalogItems
{
    public partial class CatalogFacade
    {

        /*  NAME:   UpdateCatalogItem
         *  DESC:   Updates the CatalogItem associated with the CMCDescription
         *  RETURN: The newly updated Catalog Item.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void UpdateCatalogItem(CMCDescription cmcDescription)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update Catalog Item [ID: {0}]: {1}", cmcDescription.ID , ex.Message), ex);
            }
        }
    }
}
