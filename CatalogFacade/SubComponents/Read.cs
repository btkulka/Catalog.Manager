using Catalog.Facade.BUILDERCatalog;
using Catalog.Facade.Wrappers;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {
        /*  NAME:   GetCatalogSubComponents
         *  DESC:   Fetches all available Catalog SubComponents
         *  RETURN: A list of all available Catalog SubComponents
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<SubComponentDescription> GetCatalogSubComponents()
        {
            List<SubComponentDescription> retval = new List<SubComponentDescription>();

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogSubComponents();

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = new List<SubComponentDescription>(result.SubCompCatalog);
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve all SubComponents: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all SubComponents: {0}", ex.Message), ex);
            }

            return retval;
        }

        /*  NAME:   GetCatalogSubComponent
         *  DESC:   Fetches the Catalog SubComponent with the given id.
         *  PARAMS: {0} subCompId - the SubComponent id
         *  RETURN: The fetched Catalog SubComponent
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRSubComponentDescription GetCatalogSubComponent(int subCompId)
        {
            WPRSubComponentDescription retval = null;

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogSubComponent(subCompId);

                if (result.Status == FunctionResultMessage.Success)
                {
                    retval = (WPRSubComponentDescription)new List<SubComponentDescription>(result.SubCompCatalog).FirstOrDefault();
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to retrieve SubComponent [ID: {0}]: {1}", subCompId, result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the requested SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);

                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }
    }
}
