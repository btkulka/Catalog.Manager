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

        /*  NAME:   getSubComponents
         *  DESC:   Retrieves all available SubComponents from CatalogFacade and converts them to CMOs.
         *  RETURN: A list of all available SubComponents as CMOs.
         */
        public List<CMOSubComponent> getSubComponents()
        {
            // Initialize results
            List<CMOSubComponent> CMOResults = new List<CMOSubComponent>();

            try
            {
                // Fetch all SubComponents
                var subComps = cf.GetCatalogSubComponents();

                // Convert all Descriptions to CMOs
                foreach(SubComponentDescription scd in subComps)
                {
                    CMOSubComponent cmo = new CMOSubComponent()
                    {
                        CompUnitsId = scd.CompUnitsID,
                        Description = scd.Description,
                        StatusCode = "200",
                        StatusDetail = "Success",
                        SubComponentId = scd.ID
                    };

                    CMOResults.Add(cmo);
                }
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while trying to fetch all available SubComponents: {0}", ex.Message), ex);
            }

            return CMOResults;
        }

       /*  NAME:    getSubComponent
        *  DESC:    Retrieves the SubComponent associated with the given Id
        *  PARAM:   {0} subCompId - the Id of the requested SubComponent
        *  RETURN:  The requested SubComponent as a CMO.
        */
        public CMOSubComponent getSubComponent(int subCompId)
        {
            // Initialize results
            CMOSubComponent Result = null;

            try
            {
                // Fetch SubComponent
                var subComp = cf.GetCatalogSubComponent(subCompId);

                // Convert to CMO
                CMOSubComponent cmo = new CMOSubComponent()
                {
                    CompUnitsId = subComp.CompUnitsID,
                    Description = subComp.Description,
                    StatusCode = "200",
                    StatusDetail = "Success",
                    SubComponentId = subComp.ID
                };

                Result = cmo;
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while trying to fetch SubComponent [ID: {0}]: {1}", subCompId, ex.Message), ex);
            }

            return Result;
        }
    }
}
