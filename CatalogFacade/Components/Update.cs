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

        /*  NAME:   UpdateCatalogComponent
         *  DESC:   Updates the given Catalog Component.
         *  PARAMS: {0} compUpdates - a ComponentCatalogDescription object with all fields to update
         *  RETURN: The newly updated Catalog Component within a Wrapper to communicate success.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: add reference for ComponentCatalogDescription
        public WPRComponentDescription UpdateCatalogComponent(ComponentCatalogDescription compUpdates)
        {
            WPRComponentDescription Return = WPRComponentDescription.Wrap(compUpdates);
            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCatalogComponent(compUpdates);

                if(result == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update the passed Catalog Component: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the requested System [ID: {0}]: {1}", compUpdates.ID, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }

    }
}
