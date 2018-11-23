using CatalogData.Exceptions;
using System;
using Catalog.Facade.BUILDERCatalog;
using System.Collections.Generic;
using System.Text;
using Catalog.Facade.Wrappers;

namespace CatalogFacade
{
    public partial class CatalogFacade
    {

        /*  NAME:   CreateCatalogItem
         *  DESC:   Creates a new Catalog Item (RO_CMC) in the BUILDER database.
         *  PARAMS: {0} ?
         *  RETURN: The newly created Catalog Item as a WPRCMCDescription in order to get the call status.
        */
        public WPRCMCDescription CreateCatalogItem(CMCDescription desc)
        {
            WPRCMCDescription retval = (WPRCMCDescription)desc;
            try
            {
                var cc = InitCatalogClient();
                var result = cc.CreateCMC(
                        desc.ID,
                        desc.CompLink,
                        desc.MCatLink,
                        desc.CompLink,
                        desc.UoM,
                        desc.CompUIILink,
                        (float)desc.AdjFactor,
                        (float)desc.CII,
                        (int)desc.UseAltDesc,
                        (int)desc.TaskListLink,
                        (Guid)desc.CIISetLink
                    );

                if(result.Status == FunctionResultMessage.Success)
                {
                    retval.StatusCode = "200";
                    retval.StatusDetail = "Success";
                }
                else
                {
                    throw new CMBUILDERResponseException(String.Format("BUILDER was unable to create CatalogItem: {0}", result.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMCatalogFacadeException(String.Format("Catalog Manager encountered an error while trying to create the given CatalogItem: {0}", ex.Message), ex);

                retval.StatusCode = ((CMException)ex).ErrorCode;
                retval.StatusDetail = ex.Message;
            }

            return retval;
        }
    }
}
