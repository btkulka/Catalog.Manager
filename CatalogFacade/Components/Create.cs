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
        /*  NAME:   CreateCatalogComponent
         *  DESC:   Creates a new Catalog Component in the BUILDER database.
         *  PARAMS: {0} desc - a ComponentDescription object that holds all required fields
         *  RETURN: The newly created Catalog System in a wrapper to communicate Status.
        */
        public WPRComponentDescription CreateCatalogComponents(ComponentCatalogDescription desc)
        {
            WPRComponentDescription Return = (WPRComponentDescription)desc;
            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCatalogComponent(desc.ID, desc.Description, desc.SysId, (bool)desc.IsUII, desc.UIICode, (bool)desc.IsEquip);

                if(result.Status == FunctionResultMessage.Success)
                {
                    Return.StatusCode = "200";
                    Return.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to create Catalog Component: {0}", result.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given Component [Desc: {0}]: {1}", desc.Description, ex.Message), ex);

                Return.StatusCode = ((CMException)ex).ErrorCode;
                Return.StatusDetail = ex.Message;
            }

            return Return;
        }
    }
}
