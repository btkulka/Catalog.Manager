using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Facade.Wrappers;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   UpdateCatalogSystem
         *  DESC:   Updates the given Catalog System.
         *  PARAMS: {0} sysUpdates - a SystemCatalogDescription object with all fields to update
         *  RETURN: The newly updated Catalog System in a Wrapper to communicate Status
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: add reference for SystemCatalogDescription
        public WPRSystemDescription UpdateCatalogSystem(SystemCatalogDescription sysUpdates)
        {
            WPRSystemDescription Return = new WPRSystemDescription();
            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCatalogSystem(sysUpdates);

                if(result == FunctionResultMessage.Success)
                {
                    Return = WPRSystemDescription.Wrap(sysUpdates);
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update the Catalog System: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested System [ID: {0}]: {1}", sysUpdates.ID, ex.Message), ex);

                Return.StatusDetail = ex.Message;
                Return.StatusCode = ((CMException)ex).ErrorCode;
            }

            return Return;
        }
    }
}
