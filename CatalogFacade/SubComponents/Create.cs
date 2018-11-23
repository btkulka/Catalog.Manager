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
        /*  NAME:   CreateCatalogSubComponent
         *  DESC:   Creates a new Catalog SubComponent in the BUILDER database.
         *  PARAMS: {0} desc - a SubComponentDescription object with all required fields populated
         *          {1} unitId - the id of the unit type
         *  RETURN: The newly created Sub Component
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRSubComponentDescription CreateCatalogSubComponent(SubComponentDescription desc)
        {
            WPRSubComponentDescription Return = WPRSubComponentDescription.Wrap(desc);
            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCatalogSubComponent(desc.ID, desc.Description, desc.CompUnitsID);

                if(result.Status == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to create a SubComponent: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the requested SubComponent [ID: {0}]: {1}", desc.Description, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
