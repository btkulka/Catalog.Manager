using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Facade.BUILDERCatalog;
using CatalogData.Classes.CMOs;
using CatalogData.Exceptions;

namespace CatalogBusiness
{
    public partial class CatalogBusiness
    {
        /*  NAME:   getAllCMC
         *  DESC:   Retrieves all Catalog Items from the CatalogFacade, converts them into CatalogItems to return
         *  RETURN: A list of all available CatalogItems
         */
        public List<CMOCatalogItem> getAllCMC()
        {
            try
            {
                // Declare results
                List<CMOCatalogItem> catalogItems = new List<CMOCatalogItem>();

                // Fetch all CMCResults
                var allCMC = cf.GetAllCatalogItems();

                // Convert all descriptions to CatalogItems
                foreach (CMCDescription desc in allCMC)
                {
                    CMOCatalogItem item = new CMOCatalogItem()
                    {
                        CMCID = desc.ID,
                        adjFactor = (float)desc.AdjFactor,
                        CII = (float)desc.CII,
                        ciiSetLink = desc.CIISetLink,
                        ComponentId = desc.CompLink,
                        ComponentTypeId = desc.CTypeLink,
                        MaterialCategoryId = desc.MCatLink,
                        taskListId = desc.TaskListLink,
                        uiiId = desc.CompUIILink,
                        UoM = desc.UoM,
                        useAltDesc = desc.UseAltDesc,
                        StatusCode = "200",
                        StatusDetail = "Success"
                    };

                    catalogItems.Add(item);
                }

                // Return
                return catalogItems;
            }catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("CatalogManager experienced an issue while attempting to gather all CMC from BUILDER: {0}", ex.Message), ex);

                throw ex;
            }
        }

        /*  NAME:   getCatalogItem
         *  DESC:   Fetches a single CatalogItem from the CatalogFacade.
         *  PARAM:  {0} cmcId - id of the requested CatalogItem
         *  RETURN: Returns the CMOCatalogItem associated with the passed CMCID
         */
        public CMOCatalogItem getCatalogItem(int cmcId)
        {
            try
            {
                // Fetch the item
                var item = cf.GetCatalogItem(cmcId);

                // Convert to CatalogItem
                CMOCatalogItem cmo = new CMOCatalogItem()
                {
                    CMCID = item.ID
                };

                return cmo;
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an error while attempting to fetch Catalog Item from BUILDER [ID: {0}]: {1}", cmcId, ex.Message), ex);

                throw ex;
            }
        }

    }
}
