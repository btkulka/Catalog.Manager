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

        /*  NAME:   getComponentTypes
         *  DESC:   Retrieves all available ComponentTypes from the Catalog Facade and converts them to CMOs.
         *  RETURN: All available Component Types as a list of CMOs.
         */
        public List<CMOComponentType> getComponentTypes()
        {
            // Initialize results
            List<CMOComponentType> CMOResults = new List<CMOComponentType>();

            try
            {
                // Fetch all component types
                var compTypes = cf.GetCatalogComponentTypes();

                // convert each description to a CMO
                foreach(ComponentTypeDescription ctd in compTypes)
                {
                    CMOComponentType cmo = new CMOComponentType()
                    {
                        ComponentTypeId = ctd.ID,
                        Description = ctd.Description,
                        StatusCode = "200",
                        StatusDetail = "Success"
                    };

                    CMOResults.Add(cmo);
                }
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while trying to fetch all available Material Categories: {0}", ex.Message), ex);
            }

            return CMOResults;
        }

        /*  NAME:   getComponentTypes
         *  DESC:   Retrieves all available ComponentTypes from the Catalog Facade and converts them to CMOs.
         *  PARAM:  {0} compTypeId - the Id 
         *  RETURN: All available Component Types as a list of CMOs.
         */
        public CMOComponentType getComponentType(int compTypeId)
        {
            // Initialize result
            CMOComponentType Result = null;

            try
            {
                // Fetch ComponentType from CatalogFacade
                var compType = cf.GetCatalogComponentType(compTypeId);

                CMOComponentType cmo = new CMOComponentType()
                {
                    ComponentTypeId = compType.ID,
                    Description = compType.Description,
                    StatusCode = "200",
                    StatusDetail = "Success"
                };

                Result = cmo;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while trying to fetch all available Material Categories: {0}", ex.Message), ex);
            }

            return Result;
        }
    }
}
