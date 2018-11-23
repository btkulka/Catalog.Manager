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

        /*  NAME:   getSystems
         *  DESC:   Retrieves all available Systems from the CatalogFacade and converts them to CMOs.
         *  RETURN: All available Systems as a list of CMOs.
         */
        public List<CMOSystem> getSystems()
        {
            List<CMOSystem> CMOResults = new List<CMOSystem>();

            try
            {
                // Fetch all Catalog Systems
                var systems = cf.GetCatalogSystems();

                // Convert each to CMOs
                foreach(SystemCatalogDescription scd in systems)
                {
                    CMOSystem cmo = new CMOSystem()
                    {
                        Description = scd.Description,
                        IsUII = scd.IsUII,
                        StatusCode = "200",
                        StatusDetail = "Success",
                        SystemId = scd.ID,
                        UIICode = scd.UIICode
                    };

                    CMOResults.Add(cmo);
                }
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while attempting to fetch all Systems: {0}", ex.Message));

                throw ex;
            }

            return CMOResults;
        }

        /*  NAME:   getSystem
         *  DESC:   Retrieves the System associated with the given SysId and returns it as a CMO.
         *  PARAM:  {0} sysId - the requested System's Id
         *  RETURN: All available Systems as a list of CMOs.
         */
        public CMOSystem getSystem(int sysId)
        {
            CMOSystem Result = new CMOSystem();

            try
            {
                // Fetch system
                var system = cf.GetCatalogSystem(sysId);

                // Convert to CMO
                CMOSystem cmo = new CMOSystem()
                {
                    UIICode = system.UIICode,
                    SystemId = system.ID,
                    StatusCode = "200",
                    StatusDetail = "Success",
                    Description = system.Description,
                    IsUII = system.IsUII
                };

                Result = cmo;
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager encountered an error while attempting to Catalog System [ID: {0}]: {1}", sysId ,ex.Message));

                throw ex;
            }

            return Result;
        }

    }
}
