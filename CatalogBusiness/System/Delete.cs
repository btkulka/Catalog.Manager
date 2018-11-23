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
       /*  NAME:   deleteSystem
        *  DESC:   Calls the CatalogFacade to delete the requested System
        *  PARAM:  {0} systemId - the Id of the System to be deleted
        *  RETURN: a boolean indicating whether or not the call was successful.
        */
        public bool deleteSystem(int systemId)
        {
            try
            {
                return cf.DeleteCatalogSystem(systemId);
            }
            catch(Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager experienced an issue while attempting to delete System [ID: {0}]: {1}", systemId, ex.Message), ex);

                return false;
            }
        }

    }
}
