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

        /*  NAME:   CreateCatalogSystem
         *  DESC:   Creates a new Catalog System in the BUILDER database.
         *  PARAMS: {0} description - a description of the system
         *          {1} uiiCode - the uiiCode that identifies the system in the catalog
         *          {2} isUII - indicates if the system is UII format
         *  RETURN: The newly created Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRSystemDescription CreateCatalogSystems(SystemCatalogDescription desc)
        {
            WPRSystemDescription Return = WPRSystemDescription.Wrap(desc);
            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCatalogSystem(desc.ID, desc.Description, desc.UIICode, desc.IsUII);

                if(result.Status == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to create Catalog System: {0}", result.Status.ToString()));
                }
            }catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given System [Desc: {0}]: {1}", desc.Description, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode.ToString();
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }

    }
}
