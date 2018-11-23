using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes.CMOs;
using CatalogData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogBusiness
{
    public partial class CatalogBusiness
    {
        /*  NAME:   createCatalogItem
         *  DESC:   Takes a CMOCatalogItem, converts it to a description, and passes it to the CatalogFacade for creation.
         *  PARAM:  {0} item - a CMOCatalogItem with a bare minimum of all required fields populated.
         *  RETURN: the same passed CMOCatalogItem with an updated Status of the call.
         */
        public CMOCatalogItem createCatalogItem(CMOCatalogItem item)
        {
            try
            {
                // Check for required fields in item
                // TODO: Find other required fields
                string missingField = "";
                if (item.MaterialCategoryId == 0)
                {
                    missingField = "MaterialCategoryId";
                }
                else if (item.ComponentId == 0)
                {
                    missingField = "ComponentId";
                }
                else if (item.ComponentTypeId == 0)
                {
                    missingField = "ComponentTypeId";
                }

                // Throw error if a missing field was encountered
                if(missingField != "")
                {
                    CMInvalidCMOFormat ex = new CMInvalidCMOFormat(String.Format("CMO is missing required field '{0}'. Create cancelled.", missingField));
                }


                // Convert to Description
                CMCDescription desc = new CMCDescription()
                {
                    CompLink = item.ComponentId,
                    MCatLink = item.MaterialCategoryId,
                    AdjFactor = item.adjFactor,
                    UseAltDesc = item.useAltDesc,
                    CII = item.CII,
                    CIISetLink = (Guid)item.ciiSetLink,
                    CompUIILink = item.uiiId,
                    CTypeLink = item.ComponentTypeId,
                    ID = item.CMCID,
                    TaskListLink = item.taskListId,
                    UoM = item.UoM
                };

                // Create item
                var retval = cf.CreateCatalogItem(desc);

                // Set status
                item.StatusCode = retval.StatusCode;
                item.StatusDetail = retval.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an"));

                CMOCatalogItem result = item;
                item.StatusCode = ((CMException)ex).ErrorCode;
                item.StatusDetail = ex.Message;
            }

            return item;
        }

    }
}
