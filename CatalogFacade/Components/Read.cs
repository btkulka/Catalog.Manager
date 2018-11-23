using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using Catalog.Facade.BUILDERCatalog;
using System.Text;
using System.Linq;
using Catalog.Facade.Wrappers;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogComponent
         *  DESC:   Returns a specific Catalog System given SystemId
         *  PARAMS: {0} componentId - a valid Id representing a Catalog Component.
         *  RETURN: The requested Catalog Component as a ComponentCatalogDescription.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRComponentDescription GetCatalogComponent(int componentId)
        {
            // Initialize result
            WPRComponentDescription retval = null;

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogComponent(componentId);

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = (WPRComponentDescription) new List<ComponentCatalogDescription>(result.ComponentCatalog).FirstOrDefault();
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to fetch the requested Component [ID: {0}]: {1}", componentId, result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested Component [ID: {0}]: {1}", componentId, ex.Message), ex);

                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }

        /*  NAME:   GetCatalogComponents
         *  DESC:   Returns all available Catalog Components.
         *  RETURN: A list of all available Catalog Components.
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<ComponentCatalogDescription> GetCatalogComponents()
        {
            List<ComponentCatalogDescription> retval = new List<ComponentCatalogDescription>();
            try
            {
                var cc = InitCatalogClient();

                var results = cc.GetCatalogComponents();

                if(results.Status == FunctionResultMessage.Success)
                {
                    retval = new List<ComponentCatalogDescription>(results.ComponentCatalog);
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve all Components: {0}", results.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Catalog Components: {0}", ex.Message), ex);
            }

            return retval;
        }
    }
}
