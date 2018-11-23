using CatalogData.Exceptions;
using CatalogData.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Facade.BUILDERCatalog;
using System.Linq;
using Catalog.Facade.Wrappers;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetAllCatalogItems
         *  DESC:   Returns all available Catalog Items.
         *  RETURN: A list of all available Catalog items in the form of CMCDescription
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<CMCDescription> GetAllCatalogItems()
        {
            // Declare return value
            List<CMCDescription> retval = new List<CMCDescription>();
            try
            {
                // Initialize Facade Client
                var cc = InitCatalogClient();

                // Make BUILDER call
                var results = cc.GetAllCMC();

                // if success
                if (results.Status == FunctionResultMessage.Success)
                {
                    // Set results
                    retval = new List<CMCDescription>(results.CMCCatalog);
                }
                else
                {
                    // BUILDER experienced an error
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve all CMC: {0}", results.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                // Cast to CMException if not already one
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Catalog Items: {0}", ex.Message), ex);

                // Do something with this exception
            }

            return retval;
        }

        /*  NAME:   GetCatalogItem
         *  DESC:   Fetches the item associated with the cmcId
         *  PARAMS: {0} cmcId - the catalog item id
         *  RETURN: The requested CatalogItem
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRCMCDescription GetCatalogItem(int cmcId)
        {
            WPRCMCDescription retval = new WPRCMCDescription();
            retval.ID = cmcId;

            try
            {
                var cc = InitCatalogClient();
                var results = cc.GetCMC(cmcId);

                if(results.Status == FunctionResultMessage.Success)
                {
                    var cmcs = new List<CMCDescription>(results.CMCCatalog);

                    // we only want one item
                    retval = (WPRCMCDescription)cmcs.FirstOrDefault();
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to fetch the requested CatalogItem: {0}", results.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch Catalog Item [Id: {0}]: {1}", cmcId, ex.Message), ex);

                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }
    }
}
