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
        /*  NAME:   UpdateCatalogSubComponent
         *  DESC:   Updates a Catalog SubComponent from the BUILDER database.
         *  PARAMS: {0} subCompUpdates - an object holding all fields to update
         *  RETURN: The newly updated Catalog SubComponent
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRSubComponentDescription UpdateCatalogSubComponent(SubComponentDescription subCompUpdates)
        {
            WPRSubComponentDescription Return = WPRSubComponentDescription.Wrap(subCompUpdates);

            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCatalogSubComponent(subCompUpdates);

                if(result == FunctionResultMessage.Success)
                {
                    Return = WPRSubComponentDescription.Wrap(cc.GetCatalogSubComponent(subCompUpdates.ID).SubCompCatalog[0]);
                    Return.StatusDetail = "Success";
                    Return.StatusCode = "200";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update this SubComponent: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested SubComponent [ID: {0}]: {1}", subCompUpdates.ID, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
