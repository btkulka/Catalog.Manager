using System;
using CatalogData.Exceptions;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Linq;
using Catalog.Facade.Wrappers;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   GetCatalogSystem
         *  DESC:   Returns a specific Catalog System given SystemId
         *  PARAMS: {0} systemId - a valid Id representing a Catalog System.
         *  RETURN: The requested Catalog System.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRSystemDescription GetCatalogSystem(int systemId)
        {
            WPRSystemDescription retval = new WPRSystemDescription();

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogSystem(systemId);

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = WPRSystemDescription.Wrap(new List<SystemCatalogDescription>(result.SystemCatalog).FirstOrDefault());
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve System [ID: {0}]: {1}", systemId, result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested System [ID: {0}]: {1}", systemId, ex.Message), ex);
                retval.ID = systemId;
                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }

        /*  NAME:   GetCatalogSystems
         *  DESC:   Returns all available Catalog Systems.
         *  RETURN: A list of all available Catalog Systems
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<SystemCatalogDescription> GetCatalogSystems()
        {
            List<SystemCatalogDescription> retval = new List<SystemCatalogDescription>();
            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogSystems();

                if (result.Status == FunctionResultMessage.Success)
                {
                    retval = new List<SystemCatalogDescription>(result.SystemCatalog);
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve all Catalog Systems: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the all Catalog Systems: {0}", ex.Message), ex);
            }

            return retval;
        }

    }
}
