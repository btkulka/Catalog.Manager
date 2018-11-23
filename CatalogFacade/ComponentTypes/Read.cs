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
        /*  NAME:   GetCatalogComponentType
         *  DESC:   Fetches the given Catalog Component Type from the BUILDER database.
         *  PARAMS: {0} compTypeId - the Id of the Component Type to be fetched.
         *  RETURN: The fetched Catalog Component Type
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public WPRComponentTypeDescription GetCatalogComponentType(int compTypeId)
        {
            WPRComponentTypeDescription retval = null;

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogComponentType(compTypeId);

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = (WPRComponentTypeDescription)new List<ComponentTypeDescription>(result.CompTypeCatalog).FirstOrDefault();
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to fetch Component Type [ID: {0}]: {1}", compTypeId, result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch the given Component Type [Id: {0}]: {1}", compTypeId, ex.Message), ex);

                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }

        /*  NAME:   GetCatalogComponentTypes
         *  DESC:   Returns all available Component Types.
         *  RETURN: A list of all Catalog Component Types
         *  ERRORS: {0} CMBUILDERResponseException - BUILDER experienced an issue.
         *          {1} CMCatalogFacadeException - an error occurred while attempting to make the call.
        */
        public List<ComponentTypeDescription> GetCatalogComponentTypes()
        {
            List<ComponentTypeDescription> retval = new List<ComponentTypeDescription>();

            try
            {
                var cc = InitCatalogClient();
                var result = cc.GetCatalogComponentTypes();

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval = new List<ComponentTypeDescription>(result.CompTypeCatalog);
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to fetch all Component Types: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to fetch all Component Types: {0}", ex.Message), ex);
            }

            return retval;
        }
    }
}
