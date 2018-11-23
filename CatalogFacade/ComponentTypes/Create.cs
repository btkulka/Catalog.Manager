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
        /*  NAME:   CreateCatalogComponentType
         *  DESC:   Creates a new Catalog Component Type in the BUILDER database.
         *  PARAMS: {0} description - a ComponentTypeDescription with all required fields.
         *  RETURN: The newly created Catalog Component Type within a wrapper to communicate Status.
        */
        public WPRComponentTypeDescription CreateCatalogComponentType(ComponentTypeDescription desc)
        {
            WPRComponentTypeDescription Return = (WPRComponentTypeDescription)desc;
            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCatalogComponentType(desc.ID, desc.Description);

                if(result.Status == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to create new Component Type: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given Component Type[Desc: {0}]: {1}", desc.Description, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
