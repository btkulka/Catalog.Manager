using CatalogData.Exceptions;
using System;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   CreateCatalogItem
         *  DESC:   Creates a new Catalog Item (RO_CMC) in the BUILDER database.
         *  PARAMS: {0} ?
         *  RETURN: The newly created Catalog Item.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public void CreateCatalogItem(int componentId, int matCatId, int componentType, int UoM, int uiiId, float adjFactor, float CII, int useAltDesc, int taskListId, Guid ciiSetLink)
        {
            var cf = InitCatalogClient(this.cred);

            try
            {
               
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given Catalog Item[ComponentId: {0}, Material Category: {1} Type: {2}]: {3}", componentId, matCatId, componentType, ex.Message), ex);
            }
        }
    }
}
