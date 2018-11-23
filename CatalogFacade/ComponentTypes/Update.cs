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
        /*  NAME:   UpdateCatalogComponentType
         *  DESC:   Updates the given Catalog Component Type
         *  PARAMS: {0} compTypeUpdates - an object with all updatable fields for Component Type
         *  RETURN: The newly updated Catalog Component Type in a Wrapper to communicate status
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        // TODO: Add reference for ComponentTypeDescription
        public WPRComponentTypeDescription UpdateCatalogComponentType(ComponentTypeDescription compTypeUpdates)
        {
            WPRComponentTypeDescription Return = WPRComponentTypeDescription.Wrap(compTypeUpdates);
            try
            {
                var cc = InitCatalogClient();
                var result = cc.UpdateCatalogComponentType(compTypeUpdates);

                if(result == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to update the Component Type: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to update the given Component Type [Id: {0}]: {1}", compTypeUpdates.ID, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
