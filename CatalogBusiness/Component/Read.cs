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
        /*  NAME:   getAllComponents
         *  DESC:   Retrieves all available Components from the Catalog Facade and converts them to CMOs.
         *  RETURN: A list of all available Catalog Components as CMOs.
         */
        public List<CMOComponent> getAllComponents()
        {
            // Initialize return value
            List<CMOComponent> CMOResults = new List<CMOComponent>();

            try
            {
                // Fetch components from CatalogFacade
                var comps = cf.GetCatalogComponents();

                // Convert each ComponentCatalogDescription to CMOComponent
                foreach(ComponentCatalogDescription ccd in comps)
                {
                    CMOComponent cmo = new CMOComponent()
                    {
                        ComponentId = ccd.ID,
                        Description = ccd.Description,
                        IsEquip = ccd.IsEquip ?? false,
                        IsUII = ccd.IsUII ?? false,
                        StatusCode = "200",
                        StatusDetail = "Success",
                        SystemId = ccd.SysId,
                        UIICode = ccd.UIICode
                    };

                    CMOResults.Add(cmo);
                }
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while retrieving all Components: {0}", ex.Message), ex);

                throw ex;
            }

            return CMOResults;
        }

        /*  NAME:   getComponent
         *  DESC:   Retrieves the Catalog Component with the associated ComponentId from the CatalogFacade.
         *  PARAM:  {0} compId - the ComponentId of the requested Catalog Component.
         *  RETURN: the requested CMOComponent
         */
        public CMOComponent getComponent(int compId)
        {
            // initialize result
            CMOComponent retval = null;

            try
            {
                // Fetch component
                var comp = cf.GetCatalogComponent(compId);

                // Convert to CMO from ComponentCatalogDescription
                CMOComponent cmo = new CMOComponent()
                {
                    ComponentId = comp.ID,
                    Description = comp.Description,
                    IsEquip = (bool)comp.IsEquip,
                    IsUII = (bool)comp.IsUII,
                    StatusCode = "200",
                    StatusDetail = "Success",
                    SystemId = comp.SysId,
                    UIICode = comp.UIICode
                };

                retval = cmo;
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while retrieving Component [ID: {0}]: {1}", compId, ex.Message), ex);

                throw ex;
            }

            return retval;
        }

    }
}
