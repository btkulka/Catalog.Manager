using Catalog.Facade.BUILDERCatalog;
using Catalog.Facade.Wrappers;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   UpdateCatalogItem
         *  DESC:   Updates the CatalogItem associated with the CMCDescription
         *  RETURN: The newly updated Catalog Item within a Wrapper to communicate status.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRCMCDescription UpdateCatalogItem(CMCDescription cmcDescription)
        {
            WPRCMCDescription Return = (WPRCMCDescription)cmcDescription;
            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCMC(cmcDescription);

                if(result == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "Success";
                    Return.StatusDetail = "200";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update the given CMC Record: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update Catalog Item [ID: {0}]: {1}", cmcDescription.ID , ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
