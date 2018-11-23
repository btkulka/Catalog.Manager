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

        /*  NAME:   createCatalogSystem
         *  DESC:   Takes a CMOSystem, converts it to a Description, then sends it to the CatalogFacade for a create.
         *  PARAM:  {0} sys - the CMOSystem to be created
         *  RETURN: The same passed CMOSystem with an updated Status.
         */
        public CMOSystem createCatalogSystem(CMOSystem sys)
        {
            try
            {
                // ensure that our CMO has the required fields
                string missingField = "";

                if (missingField != "")
                    throw new CMInvalidCMOFormat(String.Format("CMOSystem is missing the required field '{0}'.", missingField));

                // Convert to description
                SystemCatalogDescription desc = new SystemCatalogDescription()
                {
                    Description = sys.Description,
                    ID = sys.SystemId,
                    IsUII = sys.IsUII,
                    UIICode = sys.UIICode
                };

                var result = cf.CreateCatalogSystems(desc);

                sys.StatusCode = result.StatusCode;
                sys.StatusDetail = result.StatusDetail;
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while trying to create new Catalog System: {0}", ex.Message), ex);

                sys.StatusCode = ((CMException)ex).ErrorCode;
                sys.StatusDetail = ex.Message;
            }

            return sys;
        }

    }
}
