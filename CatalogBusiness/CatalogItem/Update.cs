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

        /*  NAME:   updateCatalogItem
         *  DESC:   Takes a CMO, converts it to a description, then passes it to the Facade to update
         *  PARAM:  {0} cmo - a CMOCatalogItem with all required fields
         *  RETURN: The same CMOCatalogItem with updated Status.
         */
        public CMOCatalogItem updateCatalogItem(CMOCatalogItem cmo)
        {
            try
            {
                // Ensure required fields are present
                string missingField = "";
                if (cmo.CMCID == 0)
                    missingField = "CMCID";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMO Catalog Item is missing required field '{0}'.", missingField));

                // Convert to Description
                CMCDescription desc = new CMCDescription()
                {
                    AdjFactor = cmo.adjFactor,
                    CII = cmo.CII,
                    CIISetLink = (Guid)cmo.ciiSetLink,
                    CompLink = cmo.ComponentId,
                    CompUIILink = cmo.uiiId,
                    CTypeLink = cmo.ComponentTypeId,
                    ID = cmo.CMCID,
                    MCatLink = cmo.MaterialCategoryId,
                    TaskListLink = cmo.taskListId,
                    UoM = cmo.UoM,
                    UseAltDesc = cmo.useAltDesc
                };

                var result = cf.UpdateCatalogItem(desc);

                cmo.StatusCode = result.StatusCode;
                cmo.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an issue while attempting to update a Catalog Item: {0}", ex.Message), ex);

                cmo.StatusCode = ((CMException)ex).ErrorCode.ToString();
                cmo.StatusDetail = ex.Message;
            }

            return cmo;
        }

    }
}
