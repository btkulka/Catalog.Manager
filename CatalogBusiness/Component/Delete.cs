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
        /*  NAME:   deleteCatalogComponent
         *  DESC:   Calls the CatalogFacade delete Catalog Component method
         *  PARAM:  {0} compId - the Id of the Component to be deleted
         *  RETURN: a boolean indicating if the delete was successful.
         */
        public bool deleteCatalogComponent(int compId)
        {
            try
            {
                return cf.DeleteCatalogComponent(compId);
            }
            catch (Exception ex)
            {
                if (!(ex is CMException))
                    ex = new CMBusinessException(String.Format("Catalog Manager was unable to delete given Catalog Item [ID: {0}]: {1}", compId, ex.Message));

                return false;
            }
        }

    }
}
