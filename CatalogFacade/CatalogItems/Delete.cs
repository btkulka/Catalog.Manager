﻿using CatalogData.Exceptions;
using System;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   DeleteCatalogItem
         *  DESC:   Deletes the given Catalog Item (RO_CMC) in the BUILDER database.
         *  PARAMS: {0} cmcId - the Id for the catalog item
         *  RETURN: The newly deleted Catalog Item.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public bool DeleteCatalogItem(int cmcId)
        {
            var cf = InitCatalogClient();
            try
            {
                // There is currently no return for Deletes
                var result = cf.DeleteCMC(cmcId);

                if(result == FunctionResultMessage.Success)
                {
                    return true;
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to delete Catalog Item [ID: {0}]: {1}", cmcId, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to delete the given Catalog Item [Id: {0}]: {1}", cmcId, ex.Message), ex);

                return false;
            }
        }
    }
}
